using Microsoft.AspNetCore.Mvc;
using WebMagazine.Api.Models;
using WebMagazine.Api.Services;

namespace WebMagazine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(SubscriptionService subscriptionService, ILogger<SubscriptionController> logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            try
            {
                var subscriptions = await _subscriptionService.GetAllAsync();
                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all subscriptions");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubscriptionById(int id)
        {
            try
            {
                var subscription = await _subscriptionService.GetByIdAsync(id);
                if (subscription == null)
                {
                    return NotFound();
                }
                return Ok(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching subscription by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(Subscription subscription)
        {
            try
            {
                var createdSubscription = await _subscriptionService.CreateAsync(subscription);
                return CreatedAtAction(nameof(GetSubscriptionById), new { id = createdSubscription.SubscriptionID }, createdSubscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating subscription");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, Subscription subscription)
        {
            try
            {
                if (id != subscription.SubscriptionID)
                {
                    return BadRequest();
                }

                var updatedSubscription = await _subscriptionService.UpdateAsync(subscription);
                return Ok(updatedSubscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating subscription");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            try
            {
                var subscription = await _subscriptionService.GetByIdAsync(id);
                if (subscription == null)
                {
                    return NotFound();
                }

                await _subscriptionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting subscription");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
