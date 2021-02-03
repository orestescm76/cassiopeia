using System;
using System.IO;
using CSCore;
using CSCore.Tags.ID3;
using NVorbis;
using JAudioTags;
using System.Runtime.Serialization.Formatters;
using System.Drawing;

namespace aplicacion_musica
{
    public class LectorMetadatos
    {
        private readonly VorbisReader _vorbisReader = null;
        private readonly FLACFile _FLACfile;
        private readonly ID3v2QuickInfo _mp3iD3 = null;
        private readonly IWaveSource _sonido;
        public string Artista { get; private set; }
        public string Titulo { get; private set; }
        public int Pista { get; private set; }
        public string Album { get; private set; }
        public int Año { get; private set; }
        public Image Cover { get; set; }
        public TimeSpan Duracion { get; private set; }
        public LectorMetadatos(string s)
        {
            switch (Path.GetExtension(s))
            {
                case ".mp3":
                    ID3v2 mp3tag = ID3v2.FromFile(s);
                    _mp3iD3 = new ID3v2QuickInfo(mp3tag);
                    if (_mp3iD3.Artist == "")
                        Artista = "S/N";
                    else
                        Artista = _mp3iD3.Artist;
                    if (_mp3iD3.LeadPerformers != "")
                        Artista = _mp3iD3.LeadPerformers;
                    Titulo = _mp3iD3.Title;
                    Album = _mp3iD3.Album;
                    Pista = _mp3iD3.TrackNumber ?? 0;
                    Año = _mp3iD3.Year ?? 0;
                    if (_mp3iD3.Image != null)
                        Cover = _mp3iD3.Image;
                    else
                        Cover = null;
                    _sonido = CSCore.Codecs.CodecFactory.Instance.GetCodec(s).ToSampleSource().ToStereo().ToWaveSource(16);
                    Duracion = _sonido.GetLength();
                    _sonido.Dispose();
                    break;
                case ".flac":
                    _FLACfile = new FLACFile(s, true);
                    Artista = _FLACfile.ARTIST;
                    Titulo = _FLACfile.TITLE;
                    Album = _FLACfile.ALBUM;
                    Pista = Convert.ToInt32(_FLACfile.TRACKNUMBER);
                    try
                    {
                        Año = Convert.ToInt32(_FLACfile.DATE);
                    }
                    catch (FormatException)
                    {
                        Log.Instance.ImprimirMensaje("No se ha podido extraer la fecha del FLAC", TipoMensaje.Advertencia);
                        Año = 0;
                    }
                    _sonido = CSCore.Codecs.CodecFactory.Instance.GetCodec(s).ToSampleSource().ToStereo().ToWaveSource(16);
                    Duracion = _sonido.GetLength();
                    _sonido.Dispose();
                    break;
                case ".ogg":
                    _vorbisReader = new VorbisReader(s);
                    Artista = _vorbisReader.Tags.Artist;
                    Titulo = _vorbisReader.Tags.Title;
                    Album = _vorbisReader.Tags.Album;
                    try
                    {
                        Año = Convert.ToInt32(_vorbisReader.Tags.Dates[0]);
                    }
                    catch (FormatException)
                    {
                        Log.Instance.ImprimirMensaje("No se ha podido extraer la fecha del OGG", TipoMensaje.Advertencia);
                        Año = 0;
                    }
                    Duracion = _vorbisReader.TotalTime;
                    _vorbisReader.Dispose();
                    break;
                default:
                    break;
            }
            _sonido = null;
        }
        public bool Evaluable()
        {
            return ((Artista != null) && (Titulo != null)) ? true : false;
        }
    }
}
