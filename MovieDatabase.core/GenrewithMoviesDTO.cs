using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.core
{
    public class GenrewithMoviesDTO
    {
        public string Genre {  get; set; }
        public List<string> Movies { get; set; }    
    }
}
