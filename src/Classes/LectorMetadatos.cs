using System;
using System.IO;
using System.Collections;
using CSCore.Tags.ID3;
using NVorbis;
using JAudioTags;

namespace aplicacion_musica
{
    public class LectorMetadatos
    {
        private readonly VorbisReader _vorbisReader = null;
        private readonly FLACFile _FLACfile;
        private readonly ID3v2QuickInfo _mp3iD3 = null;
        public string Artista { get; private set; }
        public string Titulo { get; private set; }
        public int NumeroPista { get; private set; }

        public LectorMetadatos(string s)
        {
            NumeroPista = 0;
            switch (Path.GetExtension(s))
            {
                case ".mp3":
                    ID3v2 mp3tag = ID3v2.FromFile(s);
                    _mp3iD3 = new ID3v2QuickInfo(mp3tag);
                    Artista = _mp3iD3.LeadPerformers;
                    Titulo = _mp3iD3.Title;
                    NumeroPista = (int)_mp3iD3.TrackNumber;
                    break;
                case ".flac":
                    _FLACfile = new FLACFile(s, true);
                    Artista = _FLACfile.ARTIST;
                    Titulo = _FLACfile.TITLE;
                    NumeroPista = Convert.ToInt32(_FLACfile.TRACKNUMBER);
                    break;
                case ".ogg":
                    _vorbisReader = new VorbisReader(s);
                    foreach (String meta in _vorbisReader.Comments)
                    {
                        if (meta.Contains("TITLE="))
                            Titulo=meta.Replace("TITLE=", "");
                        else if (meta.Contains("TITLE=".ToLower()))
                            Titulo=meta.Replace("title=", "");
                    }
                    foreach (String meta in _vorbisReader.Comments)
                    {
                        if (meta.Contains("ARTIST="))
                            Artista = meta.Replace("ARTIST=", "");
                        else if (meta.Contains("ARTIST=".ToLower()))
                            Artista = meta.Replace("artist=", "");
                    }
                    Cerrar();
                    break;
                default:
                    break;
            }
        }
        private void Cerrar()
        {
            if (_vorbisReader != null)
                _vorbisReader.Dispose();
        }
    }
}
