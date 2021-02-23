using System;
using Newtonsoft.Json;

namespace Cassiopeia
{
    public class Genre
    {
        public String Id { get; set; }

        [JsonIgnore]
        public String Name { get; set; }

        public Genre(String i) { Id = i; Name = ""; }
    }
}