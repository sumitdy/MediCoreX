using MediCoreX.Api.DTOs;
using MediCoreX.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCoreX.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        // 🔐 ADMIN ONLY — View all patients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — View a patient
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Filter by age
        [HttpGet("above-age/{age}")]
        public async Task<IActionResult> GetAboveAge(int age)
        {
            var result = await _service.GetAboveAgeAsync(age);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Filter by gender
        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetByGender(string gender)
        {
            var result = await _service.GetByGenderAsync(gender);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Search patients
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var result = await _service.SearchByNameAsync(name);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Sort patients
        [HttpGet("sort")]
        public async Task<IActionResult> SortByAge([FromQuery] bool asc = true)
        {
            var result = await _service.GetSortedByAgeAsync(asc);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Pagination
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(page, pageSize);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Add patient
        [HttpPost]
        public async Task<IActionResult> Add(CreatePatientDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Update patient
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        // 🔐 ADMIN ONLY — Delete patient
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Patient deleted successfully");
        }

        // 🔐 ADMIN ONLY — Demo endpoint
        [HttpGet("admin-data")]
        public IActionResult AdminOnly()
        {
            return Ok("Only Admin can access this endpoint");
        }
    }
}
