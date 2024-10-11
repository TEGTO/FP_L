using FP_L.Domain.Country;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FP_L.Controllers
{
    [ApiController]
    [Route("countries")]
    public class CountryController : ControllerBase
    {
        private const string API_URL = "https://api.sampleapis.com/countries/countries/";

        private readonly HttpClient httpClient;

        public CountryController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryResponse>> Get(int id, CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(API_URL + id, cancellationToken);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            response.EnsureSuccessStatusCode();

            var countryResponse = await JsonSerializer.DeserializeAsync<CountryResponse>(
                await response.Content.ReadAsStreamAsync(cancellationToken),
                options,
                cancellationToken: cancellationToken
            );

            return Ok(countryResponse);
        }
    }
}