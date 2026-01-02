using Microsoft.AspNetCore.Mvc;
using MyHealthAI.Models;
using MyHealthAI.Services;

namespace MyHealthAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAIService _aiService;

        // Servisi buraya enjekte ediyoruz (Constructor Injection)
        public AnalysisController(IAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("tahmin-et")]
        public async Task<IActionResult> Analyze([FromBody] PatientAnalysisRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _aiService.AnalyzePatientAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Gerçek hayatta loglama yapılır, şimdilik hatayı görelim
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}