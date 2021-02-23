using System;
using System.IO;
using System.Drawing;
using File = TagLib.File;

namespace Cassiopeia
{
    public class LectorMetadatos
    {
        private File MusicFile;
        public string Artista { get; private set; }
        public string Titulo { get; private set; }
        public int Pista { get; private set; }
        public string Album { get; private set; }
        public int Año { get; private set; }
        public Image Cover { get; set; }
        public TimeSpan Duracion { get; private set; }
        public LectorMetadatos(string s)
        {
            //This new library doesn't care about the file.
            MusicFile = File.Create(s);

            Artista = MusicFile.Tag.FirstPerformer;
            Titulo = MusicFile.Tag.Title;
            Album = MusicFile.Tag.Album;
            Pista = (int)MusicFile.Tag.Track;
            Duracion = MusicFile.Properties.Duration;
            Año = (int)MusicFile.Tag.Year;

            if (MusicFile.Tag.Pictures.Length != 0)
            {
                try
                {
                    byte[] ImageData = MusicFile.Tag.Pictures[0].Data.Data;
                    using (MemoryStream ms = new MemoryStream(ImageData))
                        Cover = Image.FromStream(ms);
                }
                catch (Exception)
                {

                    Log.Instance.PrintMessage("Cannot extract the internal cover.", MessageType.Warning);
                    Cover = null;
                }

            }
            Duracion = MusicFile.Properties.Duration;
            Log.Instance.PrintMessage(s + " successfully read!", MessageType.Correct);
        }

        public bool Evaluable()
        {
            return (Artista != null) && (Titulo != null);
        }

        public string GetSongTitle()
        {
            return Artista + " - " + Titulo;
        }

        public void Dispose()
        {
            MusicFile.Dispose();
        }
    }
}
