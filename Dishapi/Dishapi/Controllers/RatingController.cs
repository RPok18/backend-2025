using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dishapi.BLL.Services;
using Dishapi.Core.Dtos;

namespace Dishapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost("dish/{dishId}")]
        public async Task<ActionResult<RatingResponseDto>> CreateRating(int dishId, [FromBody] RatingCreateDto ratingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetCurrentUserId();
                var rating = await _ratingService.CreateRatingAsync(userId, dishId, ratingDto);

               
                return CreatedAtAction(nameof(GetRatingsForDish), new { dishId }, rating);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the rating" });
            }
        }

        [AllowAnonymous]
        [HttpGet("dish/{dishId}")]
        public async Task<ActionResult<List<RatingDto>>> GetRatingsForDish(
            int dishId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsForDishAsync(dishId, page, pageSize);
                return Ok(ratings);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving ratings" });
            }
        }

       

        [HttpPut("{ratingId}")]
        public async Task<ActionResult<RatingResponseDto>> UpdateRating(int ratingId, [FromBody] RatingUpdateDto ratingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetCurrentUserId();
                var updated = await _ratingService.UpdateRatingAsync(userId, ratingId, ratingDto);

                if (updated == null)
                    return NotFound(new { message = "Rating not found" });

                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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
                return StatusCode(500, new { message = "An error occurred while updating the rating" });
            }
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _ratingService.DeleteRatingAsync(userId, ratingId);

                if (!result)
                    return NotFound(new { message = "Rating not found" });

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the rating" });
            }
        }

        private string GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        User.FindFirst("sub")?.Value ??
                        User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token");

            return userId;
        }
    }
}
