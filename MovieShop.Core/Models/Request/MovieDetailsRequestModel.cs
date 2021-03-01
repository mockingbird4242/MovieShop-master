using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieShop.Core.Models.Request
{
    public class MovieDetailsRequestModel
    {
        [Required] [StringLength(150)] public string Title { get; set; }
        [StringLength(2084)] public string Overview { get; set; }
        [StringLength(2084)] public string Tagline { get; set; }
        [Range(0, 500000000)] public decimal? Budget { get; set; }
        [Range(0, 5000000000)]
        [RegularExpression("^(\\d{1,18})(.\\d{1})?$")] 
        public decimal? Revenue { get; set; }
        [Url] public string ImdbUrl { get; set; }
        [Url] public string TmdbUrl { get; set; }
        [Required] [Url] public string PosterUrl { get; set; }
        [Required] [Url] public string BackdropUrl { get; set; }
        public string OriginalLanguage { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
        public int? RunTime { get; set; }
        [Range(.99, 49)]
        public decimal? Price { get; set; }

        public List<GenreRequestModel> Genres { get; set; }
        public List<CastRequestModel> Casts { get; set; }
    }

    public class GenreRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CastRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string TmdbUrl { get; set; }
        public string ProfilePath { get; set; }
        public string Character { get; set; }
    }
}