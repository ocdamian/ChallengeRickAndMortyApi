using ChallengeRickAndMortyApi.Infrastructure.Responses;

namespace ChallengeRickAndMortyApi.Infrastructure.ExternalServices
{
    public interface IRickAndMortyApiService
    {
        public Task<List<Character>> GetCharactersAsync();
    }
}
