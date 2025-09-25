using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Profileapi.Models.DTOs;
using Profileapi.Services;

namespace Profileapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileResponseDto>> GetProfile()
        {
            var userId = GetCurrentUserId();
            var profile = await _profileService.GetProfileByUserIdAsync(userId);

            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            return Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileResponseDto>> CreateProfile(ProfileCreateDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetCurrentUserId();
                var profile = await _profileService.CreateProfileAsync(userId, profileDto);

                return CreatedAtAction(nameof(GetProfile), new { }, profile);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ProfileResponseDto>> UpdateProfile(ProfileUpdateDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var updatedProfile = await _profileService.UpdateProfileAsync(userId, profileDto);

            if (updatedProfile == null)
            {
                return NotFound("Profile not found.");
            }

            return Ok(updatedProfile);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = GetCurrentUserId();
            var result = await _profileService.DeleteProfileAsync(userId);

            if (!result)
            {
                return NotFound("Profile not found.");
            }

            return NoContent();
        }

        [HttpGet("exists")]
        public async Task<ActionResult<bool>> ProfileExists()
        {
            var userId = GetCurrentUserId();
            var exists = await _profileService.ProfileExistsAsync(userId);
            return Ok(new { exists });
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   User.FindFirst("sub")?.Value ??
                   throw new UnauthorizedAccessException("User ID not found in token.");
        }
    }
}