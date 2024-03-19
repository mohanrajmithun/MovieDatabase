using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;



namespace MovieDatabase.core
{
    public class Movie
    {
        public Movie() { 
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public ICollection<MovieGenre> MovieGenres { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Double rating { get; set; }

    }
}