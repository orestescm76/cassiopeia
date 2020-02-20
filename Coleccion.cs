using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace aplicacion_musica
{
    class Coleccion
    {
        public List<Album> albumes { get; private set; }
        public List<DiscoCompacto> cds { get; private set; }
        public Coleccion()
        {
            albumes = new List<Album>();
            cds = new List<DiscoCompacto>();
        }
        public void agregarAlbum(ref Album a) { albumes.Add(a); }
        public void quitarAlbum(ref Album a) { albumes.Remove(a); }
        /// <summary>
        /// busca álbumes con un título en concreto
        /// </summary>
        /// <param name="titulo">título del álbum a buscar</param>
        /// <returns>un array con los álbumes encontrados</returns>
        public Album[] buscarAlbum(string titulo)
        {
            Album[] encontrados = new Album[0];
            foreach(Album a in albumes)
            {
                if (a.nombre == titulo)
                    encontrados[encontrados.Length] = a;
            }
            return encontrados;
        }
        public bool estaEnColeccion(Album a)
        {
            foreach(Album album in albumes)
            {
                if (album.sonIguales(a))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// busca un álbum con formato artista_titulo
        /// </summary>
        /// <param name="s"></param>
        /// <returns>el álbum encontrado</returns>
        public Album devolverAlbum(string s)
        {
            String[] busqueda = s.Split('_');
            foreach (Album album in albumes)
            {
                if (album.artista == busqueda[0] && album.nombre == busqueda[1])
                    return album;
            }
            return null;
        }
        public void devolverAlbum(string s, out DiscoCompacto cd)
        {
            cd = null;
            String[] busqueda = s.Split('_');
            foreach (DiscoCompacto cdd in cds)
            {
                if (cdd.Album.artista == busqueda[0] && cdd.Album.nombre == busqueda[1])
                    cd = cdd;
            }
        }
        /// <summary>
        /// cambia la lista
        /// </summary>
        /// <param name="n">la nueva lista de discos</param>
        public void cambiarLista(ref List<Album> n)
        {
            albumes = n;
        }
        /// <summary>
        /// limpia los discos
        /// </summary>
        public void BorrarTodo()
        {
            albumes.Clear();
        }
        public void AgregarCD(ref DiscoCompacto cd)
        {
            cds.Add(cd);
        }
        /// <summary>
        /// devuelve un cd por id, tal vez habria que cambiar la EEDD
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DiscoCompacto getCDById(string i)
        {
            DiscoCompacto busqueda = null;
            for (int j = 0; j < cds.Count; j++)
            {
                if (i == cds[j].Id)
                    busqueda = cds[j];
            }
            return busqueda;
        }
        public void BorrarCD(DiscoCompacto cd)
        {
            cds.Remove(cd);
        }
    }
}
