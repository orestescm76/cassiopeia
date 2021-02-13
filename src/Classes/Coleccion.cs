using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace aplicacion_musica
{
    class Coleccion
    {
        public List<AlbumData> albumes { get; private set; }
        public List<DiscoCompacto> cds { get; private set; }
        public Coleccion()
        {
            albumes = new List<AlbumData>();
            cds = new List<DiscoCompacto>();
        }
        public void agregarAlbum(ref AlbumData a) { albumes.Add(a); }
        public void quitarAlbum(ref AlbumData a) 
        {
            if (a.CanBeRemoved)
                albumes.Remove(a);
            else
                throw new InvalidOperationException();
        }
        public List<AlbumData> buscarAlbum(string titulo)
        {
            List<AlbumData> encontrados = new List<AlbumData>();
            foreach(AlbumData a in albumes)
            {
                if (a.Title == titulo)
                    encontrados.Add(a);
            }
            return encontrados;
        }
        public bool estaEnColeccion(AlbumData a)
        {
            foreach(AlbumData album in albumes)
            {
                if (album.sonIguales(a))
                    return true;
            }
            return false;
        }
        public AlbumData devolverAlbum(string s)
        {
            String[] busqueda = s.Split('_');
            foreach (AlbumData album in albumes)
            {
                if (album.Artist == busqueda[0] && album.Title == busqueda[1])
                    return album;
            }
            return null;
        }
        public AlbumData devolverAlbum(int i)
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
        public void cambiarLista(ref List<AlbumData> n)
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
