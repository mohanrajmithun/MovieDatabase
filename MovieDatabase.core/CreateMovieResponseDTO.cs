using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.core
{
    public class CreateMovieResponseDTO
    {
        public string MovieName { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }  
    }
}
