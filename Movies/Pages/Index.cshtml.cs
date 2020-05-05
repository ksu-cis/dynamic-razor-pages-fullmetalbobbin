using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// The movies to display on the index page 
        /// </summary>
        public IEnumerable<Movie> Movies { get; protected set; }

        /// <summary>
        /// The current search terms 
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchTerms { get; set; } = "";

        /// <summary>
        /// The filtered MPAA Ratings
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string[] MPAARatings { get; set; }

        /// <summary>
        /// The filtered genres
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string[] Genres { get; set; }

        /// <summary>
        /// The minimum IMDB Rating
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double? IMDBMin { get; set; }

        /// <summary>
        /// The maximum IMDB Rating
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double? IMDBMax { get; set; }


        /// <summary>
        /// The minimum Rotten Rating
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double? RottenMin { get; set; }

        /// <summary>
        /// The maximum Rotten Rating
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public double? RottenMax { get; set; }

        /// <summary>
        /// Gets the search results for display on the page
        /// </summary>
        public void OnGet(double? IMDBMin, double? IMDBMax, double? RottenMin, double? RottenMax)
        {
            /* 
            this.IMDBMin = IMDBMin;
            this.IMDBMax = IMDBMax;
            this.RottenMin = RottenMin;
            this.RottenMax = RottenMax;
            SearchTerms = Request.Query["SearchTerms"];
            MPAARatings = Request.Query["MPAARatings"];
            Genres = Request.Query["Genres"];
            Movies = MovieDatabase.Search(SearchTerms);
            Movies = MovieDatabase.FilterByMPAARating(Movies, MPAARatings);
            Movies = MovieDatabase.FilterByGenre(Movies, Genres);
            Movies = MovieDatabase.FilterByIMDBRating(Movies, IMDBMin, IMDBMax);
            Movies = MovieDatabase.FilterByRottenRating(Movies, RottenMin, RottenMax);
            */

            Movies = MovieDatabase.All;
            // Search movie titles for the SearchTerms
            if (SearchTerms != null)
            {
                Movies = Movies.Where(movie => 
                    movie.Title != null && 
                    movie.Title.Contains(SearchTerms, StringComparison.InvariantCultureIgnoreCase)
                    );

                /*Movies = from movie in Movies
                         where movie.Title != null && movie.Title.Contains(SearchTerms, StringComparison.InvariantCultureIgnoreCase)
                         select movie; */
            }

            // Filter by MPAA Rating 
            if (MPAARatings != null && MPAARatings.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MPAARating != null &&
                    MPAARatings.Contains(movie.MPAARating)
                    );
            }

            // Filter by Genres 
            if (Genres != null && Genres.Length != 0)
            {
                Movies = Movies.Where(movie =>
                    movie.MajorGenre != null &&
                    Genres.Contains(movie.MajorGenre)
                    );
            }

            //Filter by IMDB
            if (IMDBMin != null || IMDBMax != null)
            {
                Movies = Movies.Where(movie => movie.IMDBRating != null &&
                (
                (IMDBMin != null && IMDBMax == null && movie.IMDBRating >= IMDBMin) ||
                (IMDBMin == null && IMDBMax != null && movie.IMDBRating <= IMDBMax) ||
                (IMDBMin != null && IMDBMax != null && movie.IMDBRating >= IMDBMin && movie.IMDBRating <= IMDBMax)
                )
                );
            }


            //Filter by Rotten
            if (RottenMin != null || RottenMax != null)
            {
                Movies = Movies.Where(movie => movie.RottenTomatoesRating != null &&
                (
                (RottenMin != null && RottenMax == null && movie.RottenTomatoesRating >= RottenMin) ||
                (RottenMin == null && RottenMax != null && movie.RottenTomatoesRating <= RottenMax) ||
                (RottenMin != null && RottenMax != null && movie.RottenTomatoesRating >= RottenMin && movie.RottenTomatoesRating <= RottenMax)
                )
                );
            }


        }

    }
}
