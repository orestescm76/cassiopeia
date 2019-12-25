using System;

namespace aplicacion_ipo
{
    public class Genero
    {
        public Genero(String i) { Id = i; traducido = ""; }
        public void setTraduccion(string t) { traducido = t; }
        public String Id { get; }
        public String traducido { get; set; }
    }
}
