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
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly RoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IMapper mapper, RoleService roleService, ILogger<RolesController> logger)
        {
            _mapper = mapper;
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<RoleDTO>>(roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching roles");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<RoleDTO>(role));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching role with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoleDTO>> CreateRole(RoleDTO roleDto)
        {
            try
            {
                var role = _mapper.Map<Role>(roleDto);
                var createdRole = await _roleService.CreateAsync(role);
                return CreatedAtAction(nameof(GetRole), new { id = createdRole.RoleID }, _mapper.Map<RoleDTO>(createdRole));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new role");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleDTO roleDto)
        {
            if (id != roleDto.RoleID)
            {
                return BadRequest();
            }

            try
            {
                var role = _mapper.Map<Role>(roleDto);
                await _roleService.UpdateAsync(role);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                await _roleService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
