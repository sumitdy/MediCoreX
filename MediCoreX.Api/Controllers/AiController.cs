using MediCoreX.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCoreX.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("patient-summary")]
        public async Task<IActionResult> GeneratePatientSummary(
            [FromQuery] string fullName,
            [FromQuery] int age,
            [FromQuery] string gender)
        {
            var summary = await _aiService.GeneratePatientSummaryAsync(
                fullName,
                age,
                gender);

            return Ok(new
            {
                patientName = fullName,
                summary = summary
            });
        }
    }
}