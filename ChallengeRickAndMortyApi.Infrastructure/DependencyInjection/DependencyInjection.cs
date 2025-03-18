using ChallengeRickAndMortyApi.Application.Interfaces;
using ChallengeRickAndMortyApi.Infrastructure.Caching;
using ChallengeRickAndMortyApi.Infrastructure.ExternalServices;
using ChallengeRickAndMortyApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChallengeRickAndMortyApi.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(Log.Logger);
            serviceCollection.AddHttpClient<IRickAndMortyApiService, RickAndMortyApiService>(client =>
            {
                client.BaseAddress = new Uri("https://rickandmortyapi.com");
            });
            serviceCollection.AddScoped<ICharacterService, CharacterService>();
            serviceCollection.AddSingleton<IMemoryCacheService, MemoryCacheService>();

            return serviceCollection;
        }
    }
}
