using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using System.Security.Claims;

namespace PresenceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestsService;
        private readonly ILogger<LeaveRequestsController> _logger;

        public LeaveRequestsController(ILeaveRequestService leaveRequestsService, ILogger<LeaveRequestsController> logger)
        {
            _leaveRequestsService = leaveRequestsService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> Get()
        {
            _logger.LogInformation("Getting all leave requests.");
            var requests = await _leaveRequestsService.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<LeaveRequestDto>> GetById(int id)
        {
            _logger.LogInformation($"Getting leave request by ID: {id}");
            var dto = await _leaveRequestsService.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Leave request with ID: {id} not found.");
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LeaveRequestDto>> Post([FromBody] LeaveRequestCreateModel model)
        {
            _logger.LogInformation("Attempting to create a new leave request.");

            try
            {
                var userIdClaim = User.FindFirst("userId"); // בדקי אם זה ClaimTypes.NameIdentifier אצלך
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    _logger.LogWarning("User ID claim missing or invalid in JWT.");
                    return Unauthorized("User ID not found in token.");
                }

                var createdRequest = await _leaveRequestsService.AddRequestAsync(model, userId);

                _logger.LogInformation($"Leave request created successfully. ReqId: {createdRequest.ReqId}");
                return Ok(createdRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating leave request.");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] LeaveRequestDto leaveRequestDto)
        {
            _logger.LogInformation($"Updating leave request with ID: {id}");

            try
            {
                var updated = await _leaveRequestsService.UpdateRequestAsync(id, leaveRequestDto);
                if (updated == null)
                {
                    _logger.LogWarning($"Leave request with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Leave request with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating leave request ID: {id}");
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteRequestAsync(int id)
        {
            _logger.LogInformation($"Deleting leave request with ID: {id}");

            try
            {
                await _leaveRequestsService.DeleteRequestAsync(id);
                _logger.LogInformation($"Leave request with ID {id} deleted successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting leave request ID: {id}");
                return StatusCode(500, "Internal server error occurred.");
            }
        }
    }
}


