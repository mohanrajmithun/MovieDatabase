using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieDatabase.core
{
    public class MovieGenre
    {
        
        public int MovieId { get; set; }
        [ValidateNever]
        public Movie Movie { get; set; }
        public int GenreId { get; set; }
        [ValidateNever]
        public Genre Genre { get; set; }
    }
}
