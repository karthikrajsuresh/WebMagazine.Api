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
    public class TagsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TagService _tagService;
        private readonly ILogger<TagsController> _logger;

        public TagsController(IMapper mapper, TagService tagService, ILogger<TagsController> logger)
        {
            _mapper = mapper;
            _tagService = tagService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags()
        {
            try
            {
                var tags = await _tagService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<TagDTO>>(tags));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tags");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTag(int id)
        {
            try
            {
                var tag = await _tagService.GetByIdAsync(id);
                if (tag == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<TagDTO>(tag));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tag with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TagDTO>> CreateTag(TagDTO tagDto)
        {
            try
            {
                var tag = _mapper.Map<Tag>(tagDto);
                var createdTag = await _tagService.CreateAsync(tag);
                return CreatedAtAction(nameof(GetTag), new { id = createdTag.TagID }, _mapper.Map<TagDTO>(createdTag));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new tag");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, TagDTO tagDto)
        {
            if (id != tagDto.TagID)
            {
                return BadRequest();
            }

            try
            {
                var tag = _mapper.Map<Tag>(tagDto);
                await _tagService.UpdateAsync(tag);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating tag with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                var tag = await _tagService.GetByIdAsync(id);
                if (tag == null)
                {
                    return NotFound();
                }

                await _tagService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting tag with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
