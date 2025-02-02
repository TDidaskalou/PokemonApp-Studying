﻿using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool ReviewExists(int reviewId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        //ΔΗΜΙΟΥΡΓΟΥΜΕ DELETE RANGE ΤΟ ΧΡΙΕΑΖΟΜΑΣΤΕ ΣΕ ΑΥΤΗ ΤΗΝ ΠΕΡΙΠΤΩΣΗ ΓΙΑ ΟΤΑΝ ΘΑ ΚΑΝΟΥΜΕ DELETE POKEMON 
        bool DeleteReviews(List<Review> reviews);
        bool Save();

    }
}
