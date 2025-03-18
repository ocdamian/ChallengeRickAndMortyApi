using ChallengeRickAndMortyApi.Application.DTOs;
using ChallengeRickAndMortyApi.Domain.Entities;

namespace ChallengeRickAndMortyApi.Application.Interfaces
{
    public interface ICharacterService
    {
        public Task<List<Personaje>> GetCharactersAsync();
        public Task<List<TopSpecies>> GetTopSpeciesAsync();
    }
}
