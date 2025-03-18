using ChallengeRickAndMortyApi.Infrastructure.Responses;
using Serilog;
using System.Net.Http.Json;

namespace ChallengeRickAndMortyApi.Infrastructure.ExternalServices
{
    public class RickAndMortyApiService : IRickAndMortyApiService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public RickAndMortyApiService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            try
            {
                _logger.Information("Iniciando solicitud a la API de Rick and Morty en {Time}", DateTime.UtcNow);
                var response = await _httpClient.GetAsync("api/character");

                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.Error("Respuesta no exitosa de la API externa. Código de estado: {StatusCode}", response.StatusCode);
                    throw new Exception($"Error al obtener personajes. Código de estado: {response.StatusCode}");
                }

                var body = await response.Content.ReadFromJsonAsync<RickAndMortyResponse>();

                if (body == null)
                {
                    _logger.Error("Respuesta vacía o malformada recibida de la API.");
                    throw new Exception("La respuesta de la API es nula o malformada.");
                }

                return body?.Results ?? [];

            }
            catch (HttpRequestException ex)
            {
                _logger.Error(ex, "Error en la solicitud HTTP al obtener personajes de la API externa.");
                throw new HttpRequestException("Error al realizar la solicitud HTTP a la API externa.", ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado al obtener los personajes de la API.");
                throw new Exception("Error inesperado al obtener los personajes de la API externa.", ex);
            }
        }
    }
}
