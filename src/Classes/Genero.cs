using System;
using Newtonsoft.Json;

namespace aplicacion_musica
{
    public class Genero
    {
        public String Id { get; set; }
        [JsonIgnore]
        public String traducido { get; set; }
        public Genero() { }
        public Genero(String i) { Id = i; traducido = ""; }
        public void setTraduccion(string t) { traducido = t; }
    }
}
