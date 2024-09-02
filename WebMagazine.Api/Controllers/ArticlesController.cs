using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;
using WebMagazine.Api.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebMagazine.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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
                    _logger.LogWarning("Article with ID {Id} not found.", id);
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

        [HttpGet("{id}/get-image")]
        public async Task<IActionResult> GetImage(int id)
        {
            try
            {
                var base64Image = await _articleService.GetImageAsBase64StringAsync(id);
                return Ok(new { ImageBase64 = base64Image });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving image");
                return BadRequest(ex.Message);
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

        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, [FromBody] string base64Image)
        {
            try
            {
                var image = await _articleService.UploadImageAsync(id, base64Image);
                return Ok(new { ImageBase64 = image });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading image");
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("{id}/upload-file")]
        //public async Task<IActionResult> UploadImageFile(int id, IFormFile imageFile)
        //{
        //    if (imageFile.Length > 0)
        //    {
        //        using var ms = new MemoryStream();
        //        await imageFile.CopyToAsync(ms);
        //        var fileBytes = ms.ToArray();
        //        string base64Image = Convert.ToBase64String(fileBytes);
        //        try
        //        {
        //            await _articleService.UploadImageAsync(id, base64Image);
        //            return Ok(new { Message = "Image uploaded successfully" });
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error occurred while uploading image file");
        //            return BadRequest(ex.Message);
        //        }
        //    }
        //    return BadRequest("Invalid file upload attempt.");
        //}

        //[HttpPost("{id}/upload-file")]
        //public async Task<IActionResult> UploadImageFile(int id, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }

        //    try
        //    {
        //        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);
        //        using (var stream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        return Ok($"File {file.FileName} has been uploaded successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

        [HttpPost("{id}/upload-file")]
        public async Task<IActionResult> UploadImageFile(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Ensure the directory exists
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Save the file to the directory
                var filePath = Path.Combine(directoryPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Convert the saved image to a base64 string
                byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                string base64Image = Convert.ToBase64String(imageBytes);

                // Save the base64 string in the database
                await _articleService.UploadImageAsync(id, base64Image);

                return Ok(new { Message = $"File {file.FileName} has been uploaded successfully and stored as base64 in the database." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading image file");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}/download-image")]
        public async Task<IActionResult> DownloadImage(int id)
        {
            try
            {
                var base64Image = await _articleService.GetImageAsBase64StringAsync(id);
                byte[] imageBytes = Convert.FromBase64String(base64Image);
                return File(imageBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading image");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDTO articleDto)
        {
            if (id != articleDto.ArticleID)
            {
                _logger.LogWarning("Article with ID {Id} not found for Update.", id);
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
                    _logger.LogWarning("Article with ID {Id} not found for Deletion.", id);
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
