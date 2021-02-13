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
        public void quitarAlbum(ref Album a) 
        {
            if (a.PuedeBorrarse)
                albumes.Remove(a);
            else
                throw new InvalidOperationException();
        }
        public List<Album> buscarAlbum(string titulo)
        {
            List<Album> encontrados = new List<Album>();
            foreach(Album a in albumes)
            {
                if (a.Title == titulo)
                    encontrados.Add(a);
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
        public Album devolverAlbum(string s)
        {
            String[] busqueda = s.Split('_');
            foreach (Album album in albumes)
            {
                if (album.Artist == busqueda[0] && album.Title == busqueda[1])
                    return album;
            }
            return null;
        }
        public Album devolverAlbum(int i)
        {
            return albumes[i];
        }
        public void devolverAlbum(string s, out DiscoCompacto cd)
        {
            cd = null;
            String[] busqueda = s.Split('_');
            foreach (DiscoCompacto cdd in cds)
            {
                if (cdd.Album.Artist == busqueda[0] && cdd.Album.Title == busqueda[1])
                    cd = cdd;
            }
        }
        public void cambiarLista(ref List<Album> n)
        {
            albumes = n;
        }
        public void BorrarTodo()
        {
            albumes.Clear();
            cds.Clear();
        }
        public void AgregarCD(ref DiscoCompacto cd)
        {
            cds.Add(cd);
        }
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
        public void BorrarCD(String id)
        {
            foreach (DiscoCompacto item in cds)
            {
                if(item.Id == id)
                {
                    cds.Remove(item);
                    return;
                }
            }
        }
        public void BorrarCD(ref DiscoCompacto cual)
        {
            cds.Remove(cual);
        }
    }
}
