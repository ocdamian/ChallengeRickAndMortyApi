
using ChallengeRickAndMortyApi.Application.Interfaces;
using ChallengeRickAndMortyApi.Infrastructure.Services;
using ChallengeRickAndMortyApi.Domain.Entities;
using Moq;
using ChallengeRickAndMortyApi.Infrastructure.ExternalServices;
using ChallengeRickAndMortyApi.Infrastructure.Responses;
using Moq.Protected;
using Serilog;

namespace ChallengeRickAndMortyApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetCharactersAsync_ReturnsCharacters()
        {

            // Arrange (organizar)
            var mockCacheService = new Mock<IMemoryCacheService>();
            var mockApiService = new Mock<IRickAndMortyApiService>();

            var fackeCharacters = new List<Character>()
            {
              new Character {
                  Id = 1, Name = "Rick Sanchez",
                  Status = "Alive",
                  Species= "Human",
                  Type= "",
                  Gender= "Male",
                  Origin = new Origin {
                   Name = "Earth (C-137)",
                   Url = "https://rickandmortyapi.com/api/location/1"
                  },
                  Location = new Location {
                   Name = "Citadel of Ricks",
                   Url = "https://rickandmortyapi.com/api/location/3"
                  },
                  Image = "https://rickandmortyapi.com/api/character/avatar/1.jpeg",
                  Episode = new List<string>() { "https://rickandmortyapi.com/api/episode/1" },
                  Url = "https://rickandmortyapi.com/api/character/1",
                  Created = DateTime.Now
              }

            };

            mockApiService.Setup(service => service.GetCharactersAsync()).ReturnsAsync(fackeCharacters);

            // Configurar el mock del cache para que primero no tenga datos y luego los almacene
            mockCacheService.Setup(cache => cache.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<List<Personaje>>>>(),
                It.IsAny<int>()
            )).Returns<string, Func<Task<List<Personaje>>>, int>(async (key, fetchFunction, duration) => await fetchFunction());

            var characterService = new CharacterService(mockCacheService.Object, mockApiService.Object);

            // Act (Actuar)
            var result = await characterService.GetCharactersAsync();

            // Assert (Afirmar)
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("Rick Sanchez", result[0].Nombre);
        }

        [Fact]
        public async Task GetCharactersAsync_WhenApiFails_ThrowsHttpRequestException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Simular que la API lanza una HttpRequestException
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("API failure"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://rickandmortyapi.com/")
            };

            var mockLogger = new Mock<ILogger>();
            var apiService = new RickAndMortyApiService(httpClient, mockLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => apiService.GetCharactersAsync());
        }

    }
}