
namespace ChallengeRickAndMortyApi.Application.Interfaces
{
    public interface IMemoryCacheService
    {
        public Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> fetchFunction, int durationInSeconds= 120);
    }
}
