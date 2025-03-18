using ChallengeRickAndMortyApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeRickAndMortyApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CharactersController(ICharacterService characterService) : Controller
    {
        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            var characters = await characterService.GetCharactersAsync();
            return Ok(new
            {
                TotalPersonajes = characters.Count,
                Personajes = characters
            });
        }

        [HttpGet("top-species")]
        public async Task<IActionResult> GetTopSpeciesAsync()
        {
            var characters = await characterService.GetTopSpeciesAsync();
            return Ok(characters);
        }
    }
}
