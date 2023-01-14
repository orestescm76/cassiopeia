using Cassiopeia.src.Classes;
using CSCore.CoreAudioAPI;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Windows.Media.Capture;

namespace Cassiopeia.src.Player
{
    public class LocalPlayer : IPlayer
    {
        private readonly PlayerKernel PlayerKernel;
        private readonly ObservableCollection<MMDevice> AudioDevices;
        public PlayingState State { get; set; }
        public TimeSpan Duration { get => PlayerKernel.Duracion(); }
        public TimeSpan Position { get => PlayerKernel.Posicion(); }
        public bool Shuffle { get; set; }
        public float Volume { get; set; }
        public int PlaylistPointer { get; set; }
        public Playlist Playlist { get; set; }

        private Song CurrentSong;
        private bool ShuffleState = false;
        private Random Random;

        public event EventHandler SongChanged;

        public LocalPlayer()
        {
            Log.Instance.PrintMessage("Loading player in local mode", MessageType.Info);
            PlayerKernel = new PlayerKernel();
            AudioDevices = new ObservableCollection<MMDevice>();
            Volume = 0.5f;
            PlaylistPointer = -1;

            Random = new();
        }

        void IDisposable.Dispose()
        {
            Stop();
            PlayerKernel.Shutdown();
            State = PlayingState.Stop;
        }

        public void HandleDragDrop()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            using (var enumerador = new MMDeviceEnumerator())
            {
                using var mmColeccion = enumerador.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
                foreach (var item in mmColeccion)
                {
                    AudioDevices.Add(item);
                }
            }
        }

        public void Next()
        {
            if (Playlist is null)
                return;

            if (Playlist.IsOutside(PlaylistPointer))
            {
                Stop();
                State = PlayingState.Stop;
            }
            else
            {
                int listaAux = PlaylistPointer;
                try
                {
                    if (!ShuffleState)
                        PlaylistPointer++;
                    else
                        PlaylistPointer = Random.Next(Playlist.Songs.Count);

                    //if (!ModoCD)
                    //    PlaySong(Playlist.GetSong(PlaylistPointer));
                    //else
                    PlaySong(PlaylistPointer);
                }
                catch (Exception ex)
                {
                    Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                    MessageBox.Show("Out of bounds!", "Player error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PlaylistPointer = listaAux;
                    return;
                }
            }
        }

        public void Pause()
        {
            PlayerKernel.Pausar();
            State = PlayingState.Paused;
        }

        public void Play()
        {
            PlayerKernel.Reproducir();
            State = PlayingState.Playing;
        }
        //private void PlaySong(string path)
        //{
        //    State = PlayingState.Stop;
        //    PlayerKernel.Shutdown();
        //    try
        //    {
        //        PlayerKernel.CargarCancion(path);
        //        MetadataSong = new LocalSong(path);                
        //        PlayerKernel.Reproducir();
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Instance.PrintMessage("A problem happened playing the song", MessageType.Error);
        //        Log.Instance.PrintMessage(e.Message, MessageType.Error);
        //        MessageBox.Show(Kernel.GetText("errorReproduccion"));
        //        return;
        //    }
        //}
        public void PlaySong(Song s)
        {
            
            State = PlayingState.Stop;

            PlayerKernel.Shutdown();
            try
            {
                PlayerKernel.CargarCancion(s.Path);
                PlayerKernel.SetVolumen(Volume);
                PlayerKernel.Reproducir();
                CurrentSong = s;
            }
            catch (Exception)
            {
                Log.Instance.PrintMessage("Cannot play", MessageType.Error);
                return;
            }
            //if (Config.HistoryEnabled)
            //{
            //    using (StreamWriter escritor = new StreamWriter(Kernel.HistorialFileInfo.FullName, true))
            //    {
            //        escritor.WriteLine(Utils.GetHistoryString(c, Kernel.SongCount));
            //        Kernel.SongCount++;
            //    }
            //}
            State = PlayingState.Playing;
        }
        void PlaySong(int Pista)
        {
            PlaylistPointer = Pista;
            State = PlayingState.Stop;
            PlaySong(Playlist.Songs[Pista]);
            //if (ModoCD)
            //    nucleo.SaltarCancionCD(Pista);
            //else
        }

        public Image GetCover()
        {
            //if we deal with a local file
            if(CurrentSong.AlbumFrom is null)
            {
                LocalSong localSong = (LocalSong)CurrentSong;
                return localSong.AlbumCover;
            }
            if (string.IsNullOrEmpty(CurrentSong.AlbumFrom?.CoverPath))
                return System.Drawing.Image.FromFile(CurrentSong.AlbumFrom.CoverPath);
            else
            {
                if (File.Exists(CurrentSong.AlbumFrom?.SoundFilesPath + "\\cover.jpg"))
                    return System.Drawing.Image.FromFile(CurrentSong.AlbumFrom.SoundFilesPath + "\\cover.jpg");
                else if (File.Exists(CurrentSong.AlbumFrom?.SoundFilesPath + "\\cover.png"))
                    return System.Drawing.Image.FromFile(CurrentSong.AlbumFrom.SoundFilesPath + "\\cover.png");
                else if (File.Exists(CurrentSong.AlbumFrom?.SoundFilesPath + "\\folder.jpg"))
                    return System.Drawing.Image.FromFile(CurrentSong.AlbumFrom.SoundFilesPath + "\\folder.jpg");
            }
            return null;
        }
        public void Previous()
        {
            if (Playlist != null && !Playlist.IsFirstSong(PlaylistPointer))
            {
                PlaylistPointer--;
                PlaySong(PlaylistPointer);

                //if (!ModoCD)
                //    PlaySong(Playlist.GetSong(PlaylistPointer));
                //else
            }
        }

        public void Seek(int where)
        {
            throw new NotImplementedException();
        }

        public void SetShuffle()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            PlayerKernel.Shutdown();

        }
        public string GetSongPlaying()
        {
            return CurrentSong.ToString();
        }

        public void PlaySong(LongSong song)
        {
            throw new NotImplementedException();
        }

        void IPlayer.PlaySong(string path)
        {
            throw new NotImplementedException();
        }

        void IPlayer.PlaySong(int Track)
        {
            throw new NotImplementedException();
        }

        public string GetSongInfo()
        {
            return PlayerKernel.GetDatos();
        }
    }
}
