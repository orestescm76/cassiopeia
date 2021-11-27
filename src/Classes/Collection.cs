using System;
using System.Collections.Generic;

namespace Cassiopeia.src.Classes
{
    public class Collection
    {
        public LinkedList<AlbumData> Albums { get; private set; }
        public List<CompactDisc> CDS { get; private set; }
        public Collection()
        {
            Albums = new LinkedList<AlbumData>();
            CDS = new List<CompactDisc>();
        }
        public void AddAlbum(ref AlbumData album) { Albums.AddFirst(album); }
        public void RemoveAlbum(ref AlbumData album) 
        {
            if (album.CanBeRemoved)
                Albums.Remove(album);
            else
                throw new InvalidOperationException(album.Artist + " - " + album.Title);
        }
        public List<AlbumData> SearchAlbum(string title)
        {
            List<AlbumData> encontrados = new List<AlbumData>();
            foreach(AlbumData a in Albums)
            {
                if (a.Title == title)
                    encontrados.Add(a);
            }
            return encontrados;
        }
        public bool IsInCollection(AlbumData referenceAlbum)
        {
            foreach(AlbumData album in Albums)
            {
                if (album == referenceAlbum)
                    return true;
            }
            return false;
        }

        public AlbumData GetAlbum(string s) //s is equal to Black Sabbath/**/Paranoid
        {
            String[] busqueda = s.Split("/**/");

            foreach (AlbumData album in Albums)
            {
                if (album.Artist == busqueda[0] && album.Title == busqueda[1])
                    return album;
            }

            return null;
        }
        public AlbumData GetAlbum(int index)
        {
            LinkedListNode<AlbumData> it = Albums.First;
            for (int i = 0; i < index; i++)
            {
                it = it.Next;
            }
            return it.Value;
            //return Albums[index];
        }
        public void GetAlbum(string s, out CompactDisc cd)
        {
            cd = null;
            String[] busqueda = s.Split("/**/");
            foreach (CompactDisc cdd in CDS)
            {
                if (cdd.Album.Artist == busqueda[0] && cdd.Album.Title == busqueda[1])
                    cd = cdd;
            }
        }
        public void ChangeList(ref LinkedList<AlbumData> n)
        {
            Albums = n;
        }
        public void Clear()
        {
            Albums.Clear();
            CDS.Clear();
        }
        public void AddCD(ref CompactDisc cd)
        {
            CDS.Add(cd);
        }
        public CompactDisc GetCDById(string i)
        {
            CompactDisc busqueda = null;
            for (int j = 0; j < CDS.Count; j++)
            {
                if (i == CDS[j].Id)
                    busqueda = CDS[j];
            }
            return busqueda;
        }
        public void DeleteCD(String id)
        {
            foreach (CompactDisc item in CDS)
            {
                if(item.Id == id)
                {
                    CDS.Remove(item);
                    ReleaseLock(item.Album);
                    return;
                }
            }
        }
        public void DeleteCD(ref CompactDisc cd)
        {
            CDS.Remove(cd);
        }
        private void ReleaseLock(AlbumData a)
        {
            //First release the lock
            a.CanBeRemoved = true;
            foreach (var cd in CDS)
            {
                //If one copy refers that album, we cannot remove
                if (cd.Album == a)
                    a.CanBeRemoved = false;
            }
        }
        public TimeSpan GetTotalTime(LinkedList<AlbumData> albums)
        {
            TimeSpan time = new TimeSpan();
            foreach (AlbumData album in albums)
            {
                time += album.Length;
            }
            return time;
        }
        public TimeSpan GetTotalTime(List<CompactDisc> CDS)
        {
            TimeSpan time = new TimeSpan();
            foreach (CompactDisc cd in CDS)
            {
                time += cd.Length;
            }
            return time;
        }
    }
}
