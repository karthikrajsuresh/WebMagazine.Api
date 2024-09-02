using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;
using WebMagazine.Api.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace WebMagazine.Api.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly NotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(IMapper mapper, NotificationService notificationService, ILogger<NotificationsController> logger)
        {
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<NotificationDTO>>(notifications));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching notifications");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDTO>> GetNotification(int id)
        {
            try
            {
                var notification = await _notificationService.GetByIdAsync(id);
                if (notification == null)
                {
                    _logger.LogWarning("Notifications with ID {Id} not found.", id);

                    return NotFound();
                }

                return Ok(_mapper.Map<NotificationDTO>(notification));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching notification with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDTO>> CreateNotification(NotificationDTO notificationDto)
        {
            try
            {
                var notification = _mapper.Map<Notification>(notificationDto);
                var createdNotification = await _notificationService.CreateAsync(notification);
                return CreatedAtAction(nameof(GetNotification), new { id = createdNotification.NotificationID }, _mapper.Map<NotificationDTO>(createdNotification));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new notification");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, NotificationDTO notificationDto)
        {
            if (id != notificationDto.NotificationID)
            {
                _logger.LogWarning("Notifications with ID {Id} not found for update.", id);

                return BadRequest();
            }

            try
            {
                var notification = _mapper.Map<Notification>(notificationDto);
                await _notificationService.UpdateAsync(notification);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating notification with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var notification = await _notificationService.GetByIdAsync(id);
                if (notification == null)
                {
                    _logger.LogWarning("Notifications with ID {Id} not found for deletion.", id);

                    return NotFound();
                }

                await _notificationService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting notification with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
