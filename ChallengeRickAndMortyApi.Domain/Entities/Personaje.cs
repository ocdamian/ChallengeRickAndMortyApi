
namespace ChallengeRickAndMortyApi.Domain.Entities
{
    public sealed class Personaje
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public int CantidadEpisodios { get; set; }
    }
}
