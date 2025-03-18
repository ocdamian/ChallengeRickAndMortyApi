using ChallengeRickAndMortyApi.Application.DTOs;
using ChallengeRickAndMortyApi.Application.Interfaces;
using ChallengeRickAndMortyApi.Domain.Entities;
using ChallengeRickAndMortyApi.Infrastructure.ExternalServices;

namespace ChallengeRickAndMortyApi.Infrastructure.Services
{
    public class CharacterService : ICharacterService
    {

        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IRickAndMortyApiService _rickAndMortyApiService;

        public CharacterService(IMemoryCacheService memoryCacheService, IRickAndMortyApiService rickAndMortyApiService)
        {
            _memoryCacheService = memoryCacheService;
            _rickAndMortyApiService = rickAndMortyApiService;
        }

        public async Task<List<Personaje>> GetCharactersAsync()
        {
            var characters = await _memoryCacheService.GetOrCreateAsync("rickandmorty-characters", async () =>
            {
                var characters = await _rickAndMortyApiService.GetCharactersAsync();
                return characters.Select(character => new Personaje
                {
                    Id = character.Id,
                    Nombre = character.Name,
                    CantidadEpisodios = character.Episode.Count,
                    Especie = character.Species,
                    Origen = character.Origin.Name,
                }).ToList();
            });

            return characters ?? [];
        }

        public async Task<List<TopSpecies>> GetTopSpeciesAsync()
        {
            var characters = await GetCharactersAsync();

            Dictionary<string, int> speciesCount = new Dictionary<string, int>();

            foreach (var character in characters)
            {
                string species = character.Especie;
                if (speciesCount.ContainsKey(species))
                    speciesCount[species]++;
                else
                    speciesCount[species] = 1;
            }

            List<TopSpecies> topSpecies = new List<TopSpecies>();
            foreach (var species in speciesCount)
            {
                topSpecies.Add(new TopSpecies { Especie = species.Key, Cantidad = species.Value });
            }

            topSpecies.Sort((a, b) => b.Cantidad.CompareTo(a.Cantidad));

            List<TopSpecies> firstFive = new List<TopSpecies>();
            int limit = Math.Min(5, topSpecies.Count);
            for (int i = 0; i < limit; i++)
            {
                firstFive.Add(topSpecies[i]);
            }

            return firstFive;
        }

        /// Podemos simplificar el cuerpo del metodo utilizando LINQ  este es un ejemplo
        /// 
        //public async Task<List<TopSpecies>> GetTopSpeciesAsync()
        //{
        //    var characters = await GetCharactersAsync();

        //    return characters
        //        .GroupBy(s => s.Especie)
        //        .Select(x => new TopSpecies { Especie = x.Key, Cantidad = x.Count() })
        //        .OrderByDescending(x => x.Cantidad)
        //        .Take(5)
        //        .ToList();
        //}

    }
}
