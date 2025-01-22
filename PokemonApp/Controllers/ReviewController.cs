using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.Dto;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Repository;

namespace PokemonApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController:Controller
    {
        
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        //IΔΙΑ ΠΕΡΙΠΤΩΣΗ ΜΕ ΤΟ OWNER
        //1η ΑΛΛΑΓΗ ΓΙΑ ERROR ΤΗΣ SQL
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository)
        {
            this._reviewRepository = reviewRepository;
            this._mapper = mapper;
            this._pokemonRepository = pokemonRepository;
            this._reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(reviews);
        }


        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokeId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        //2η ΑΛΛΑΓΗ  ΓΙΑ ERROR  ΤΗΣ SQL το  [FromQuery] int ownerId, [FromQuery] int catId,
        public IActionResult CreateReview([FromQuery] int reviewerId,[FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _reviewRepository.GetReviews().Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            //3η ΑΛΛΑΓΗ ΓΙΑ ERROR ΤΗΣ SQL προσθετουμε το Pokemon και τον Reviewr στο Review
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer =_reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully Created");
        }
    }
}
