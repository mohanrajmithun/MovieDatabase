using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDatabase.core
{
    public class MovieWithGenresDTO

    {
        public MovieWithGenresDTO() {
        
        }
        public string MovieName { get; set; }
        public List<string> Genres { get; set; }
    }
}
