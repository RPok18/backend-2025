using Microsoft.AspNetCore.Mvc;
using Dishapi.BLL.Services;
using Dishapi.Core.Dtos;
// removed invalid namespace reference
using Dishapi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using DalDish = Dishapi.DAL.Entities.Dish;
using WebDish = Dishapi.Models.Dish;


namespace Dishapi.Controllers
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
            try
            {
                var userId = GetCurrentUserId();
                var profile = await _profileService.GetProfileByUserIdAsync(userId);

                if (profile == null)
                {
                    return NotFound(new { message = "Profile not found." });
                }

                return Ok(profile);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the profile." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProfileResponseDto>> CreateProfile([FromBody] ProfileCreateDto profileDto)
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
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the profile." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _profileService.DeleteProfileAsync(userId);

                if (!result)
                {
                    return NotFound(new { message = "Profile not found." });
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the profile." });
            }
        }

        [HttpGet("exists")]
        public async Task<ActionResult<object>> ProfileExists()
        {
            try
            {
                var userId = GetCurrentUserId();
                var exists = await _profileService.ProfileExistsAsync(userId);

                return Ok(new { exists = exists });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while checking profile existence." });
            }
        }

        private string GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        User.FindFirst("sub")?.Value ??
                        User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            return userId;
        }
    }
}