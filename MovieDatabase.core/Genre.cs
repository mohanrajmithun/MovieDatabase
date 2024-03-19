using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MovieDatabase.core
{
    public  class Genre
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public ICollection<MovieGenre> MovieGenres { get; set; }



    }
}
