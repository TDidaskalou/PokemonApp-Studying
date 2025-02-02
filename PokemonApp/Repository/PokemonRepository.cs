﻿using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;

        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            //AΛΛΑΓΗ ΓΙΑ ERROR MANY TO MANY  -- ΦΤΙΑΧΝΟΥΜΕ "ΕΙΚΟΝΙΚΟΥΣ POKEMONOWNERS ΚΑΙ CATEGORY
            //KΑΙ ΑΠΟ ΤΟ CONTROLLER ΘΑ ΠΑΡΟΥΜΕ ΤΑ ΣΩΣΤΑ ID ΓΙΑ ΝΑ ΚΑΝΟΥΜΕ ΤΗΝ ΣΩΣΤΗ ΑΝΤΙΣΤΟΙΧΗΣΗ ΤΩΝ ΠΙΝΑΚΩΝ
            var pokemonOwnerEntity = _context.Owners.Where(a => a.Id ==ownerId).FirstOrDefault();
            var category = _context.Categories.Where(a=> a.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            _context.Add(pokemonCategory);
            _context.Add(pokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        //Παρολο που εχει το ιδιο ονομα είναι διαφορτικο call στο προηγουμενο ψαχναμε με το int id σε αυτο ψαχνουμε με string name
        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

            if (review.Count() <= 0)
                return 0;


            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {

            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0? true: false;  
            
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
           _context.Update(pokemon);
            return Save();
        }
    }

}