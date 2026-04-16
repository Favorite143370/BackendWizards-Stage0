using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassifyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ClassifyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name)
        {
            // 1. Validate name
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Missing or empty name parameter"
                });
            }

            // 2. Call Genderize API
            var url = $"https://api.genderize.io?name={name}";
            var response = await _httpClient.GetFromJsonAsync<GenderizeResponse>(url);

            // 3. Edge case handling
            if (response == null || response.gender == null || response.count == 0)
            {
                return StatusCode(422, new
                {
                    status = "error",
                    message = "No prediction available for the provided name"
                });
            }

            // 4. Processing rules
            int sample_size = response.count;
            double probability = response.probability;

            bool is_confident = probability >= 0.7 && sample_size >= 100;

            // 5. Final response
            return Ok(new
            {
                status = "success",
                data = new
                {
                    name = name,
                    gender = response.gender,
                    probability = probability,
                    sample_size = sample_size,
                    is_confident = is_confident,
                    processed_at = DateTime.UtcNow.ToString("o")
                }
            });
        }

        // Helper model for API response
        public class GenderizeResponse
        {
            public string? name { get; set; }
            public string? gender { get; set; }
            public double probability { get; set; }
            public int count { get; set; }
        }
    }
}