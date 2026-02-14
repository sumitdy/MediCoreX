using MediCoreX.Api.DTOs;
using MediCoreX.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCoreX.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        // 🔓 Any authenticated user
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // 🔓 Any authenticated user
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        // 🔓 Any authenticated user
        [Authorize]
        [HttpGet("above-age/{age}")]
        public async Task<IActionResult> GetAboveAge(int age)
        {
            var result = await _service.GetAboveAgeAsync(age);
            return Ok(result);
        }

        // 🔓 Any authenticated user
        [Authorize]
        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetByGender(string gender)
        {
            var result = await _service.GetByGenderAsync(gender);
            return Ok(result);
        }

        // 🔓 Any authenticated user
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var result = await _service.SearchByNameAsync(name);
            return Ok(result);
        }

        // 🔓 Any authenticated user
        [Authorize]
        [HttpGet("sort")]
        public async Task<IActionResult> SortByAge([FromQuery] bool asc = true)
        {
            var result = await _service.GetSortedByAgeAsync(asc);
            return Ok(result);
        }

        // 🔓 Any authenticated user — PAGINATION
        [Authorize]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(page, pageSize);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Add patient
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(PatientDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Delete patient
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Patient deleted successfully");
        }

        // 🔐 ADMIN ONLY — Demo endpoint
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-data")]
        public IActionResult AdminOnly()
        {
            return Ok("Only Admin can access this endpoint");
        }
    }
}
