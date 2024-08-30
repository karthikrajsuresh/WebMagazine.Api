using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;
using WebMagazine.Api.Services;
using Microsoft.Extensions.Logging;

namespace WebMagazine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CommentService _commentService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(IMapper mapper, CommentService commentService, ILogger<CommentsController> logger)
        {
            _mapper = mapper;
            _commentService = commentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
        {
            try
            {
                var comments = await _commentService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<CommentDTO>>(comments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching comments");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<CommentDTO>(comment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching comment with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CommentDTO>> CreateComment(CommentDTO commentDto)
        {
            try
            {
                var comment = _mapper.Map<Comment>(commentDto);
                var createdComment = await _commentService.CreateAsync(comment);
                return CreatedAtAction(nameof(GetComment), new { id = createdComment.CommentID }, _mapper.Map<CommentDTO>(createdComment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new comment");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, CommentDTO commentDto)
        {
            if (id != commentDto.CommentID)
            {
                return BadRequest();
            }

            try
            {
                var comment = _mapper.Map<Comment>(commentDto);
                await _commentService.UpdateAsync(comment);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating comment with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var comment = await _commentService.GetByIdAsync(id);
                if (comment == null)
                {
                    return NotFound();
                }

                await _commentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting comment with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
