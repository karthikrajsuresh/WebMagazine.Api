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
    public class ArticlesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ArticleService _articleService;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IMapper mapper, ArticleService articleService, ILogger<ArticlesController> logger)
        {
            _mapper = mapper;
            _articleService = articleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticles()
        {
            try
            {
                var articles = await _articleService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<ArticleDTO>>(articles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching articles");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> GetArticle(int id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<ArticleDTO>(article));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching article with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ArticleDTO>> CreateArticle(ArticleDTO articleDto)
        {
            try
            {
                var article = _mapper.Map<Article>(articleDto);
                var createdArticle = await _articleService.CreateAsync(article);
                return CreatedAtAction(nameof(GetArticle), new { id = createdArticle.ArticleID }, _mapper.Map<ArticleDTO>(createdArticle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new article");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDTO articleDto)
        {
            if (id != articleDto.ArticleID)
            {
                return BadRequest();
            }

            try
            {
                var article = _mapper.Map<Article>(articleDto);
                await _articleService.UpdateAsync(article);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating article with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                await _articleService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting article with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
