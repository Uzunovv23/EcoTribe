using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcoTribe.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeolocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GeolocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("reverse")]
        public async Task<IActionResult> ReverseGeocode([FromQuery] double lat, [FromQuery] double lon)
        {
            var apiKey = _configuration["ApiNinjas:Key"];
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            var url = $"https://api.api-ninjas.com/v1/reversegeocoding?lat={lat}&lon={lon}";
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Reverse geocoding failed.");
            }

            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}
