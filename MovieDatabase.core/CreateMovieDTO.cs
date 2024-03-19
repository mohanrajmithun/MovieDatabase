using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.core
{
    public class CreateMovieDTO
    {
        [Required]  
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 10)] // Assuming rating is on a scale of 0 to 10
        public double Rating { get; set; }

    }
}
