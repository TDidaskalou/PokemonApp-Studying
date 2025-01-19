using AutoMapper;
using PokemonApp.Models;

namespace PokemonApp.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
        }
    }
}
