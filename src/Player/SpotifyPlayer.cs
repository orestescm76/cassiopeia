using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cassiopeia.Properties;
using Cassiopeia.src.Classes;
using SpotifyAPI.Web;

namespace Cassiopeia.src.Player
{
    public class SpotifyPlayer : IPlayer
    {
        public PlayingState State { get; set; }
        public TimeSpan Position { get; set;}
        public TimeSpan Duration { get; set; }
        public bool Shuffle { get; set; }
        public float Volume { get; set; }
        public int PlaylistPointer { get; set; }
        public Playlist Playlist { get; set; }
        public PrivateUser User { get; private set; }
        bool SpotifyListo = false;
        public bool UserIsPremium { get; private set; }
        bool SpotifySync;
        public FullTrack PlayingSong { get; private set; }
        public string PreviousSpotifyID { get; private set; }
        private readonly Spotify SpotifyAPI = Kernel.Spotify;
        private CurrentlyPlayingContext PlayingContext;
        private readonly string CoverFileName = "./covers/np.jpg";

        public event EventHandler SongChanged;
        public event EventHandler CoverAvailable;

        public SpotifyPlayer()
        {
            Init();
            //init previoussong
            PlayingSong = new();
            PlayingSong.Id = "aaa";
            PreviousSpotifyID = "";
        }
        //Refreshes the PlayingContext.
        public void RefreshPlayingContext()
        {
            try
            {
                PlayingContext = SpotifyAPI.GetPlayingContextAsync().Result;

            }
            catch (APIException)
            {
                PlayingContext = null;
            }
            if (PlayingContext is not null)
            {
                PlayingSong = PlayingContext.Item as FullTrack;
                //Detect change in song
                if (PreviousSpotifyID != PlayingSong.Id)
                {
                    PreviousSpotifyID = PlayingSong.Id;
                    //Update new duration
                    Duration = TimeSpan.FromMilliseconds(PlayingSong.DurationMs);
                    DownloadCover(PlayingSong.Album, true);
                    SongChanged.Invoke(null, null);
                }
                Position = TimeSpan.FromMilliseconds(PlayingContext.ProgressMs);
                Volume = (float)PlayingContext.Device.VolumePercent;
                Shuffle = PlayingContext.ShuffleState;
                if (PlayingContext.IsPlaying)
                    State = PlayingState.Playing;
                else
                    State = PlayingState.Paused;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void HandleDragDrop()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            try
            {
                User = Kernel.Spotify.GetPrivateUser();
                UserIsPremium = Kernel.Spotify.UserIsPremium();
                Log.Instance.PrintMessage("Starting player with Spotify mode, e-mail: " + User.Email, MessageType.Info);
            }
            catch (APIException ex)
            {
                SpotifyListo = false;
                Log.Instance.PrintMessage(ex.Message, MessageType.Error);
                throw ex;
            }
            PlayingSong = new();

        }

        public void Next()
        {
            SpotifyAPI.SkipNext();
        }

        public void Pause()
        {
            try
            {
                SpotifyAPI.PlayResume(true);
            }
            catch (APIException ex)
            {
                MessageBox.Show(ex.Message);
            }
            State = PlayingState.Paused;
        }

        public void Play()
        {
            try
            {
                Kernel.Spotify.PlayResume(false);
            }
            catch (APIException ex)
            {
                MessageBox.Show(ex.Message);
            }
            State = PlayingState.Playing;
        }

        public void Previous()
        {
            SpotifyAPI.SkipPrevious();
        }

        public void Seek(int where)
        {
            SpotifyAPI.SeekTo(where);
        }

        public void SetShuffle()
        {
            SpotifyAPI.SetShuffle(!Shuffle);
            Shuffle = !Shuffle;
        }

        public void Stop()
        {
            Log.Instance.PrintMessage("Shutting down Spotify", MessageType.Info);
        }

        public string GetSongPlaying()
        {
            if (PlayingSong.Album is not null || PlayingSong.Name is not null)
                return PlayingSong.Artists.First().Name + " - " + PlayingSong.Name + " (" + PlayingSong.Album.Name + ")";
            else return string.Empty;
        }

        public System.Drawing.Image GetCover()
        {
            //Doing this will allow me to replace album cover and not locking the file
            if (File.Exists(CoverFileName))
            {
                using (var temp = new Bitmap(CoverFileName))
                    return new Bitmap(temp);
            }
            else return Properties.Resources.albumdesconocido;
        }

        public void PlaySong(string path)
        {
            //path is song URI in this case
            SpotifyAPI.PlaySong(path);
        }

        public void PlaySong(Song c)
        {
            throw new NotImplementedException();
        }

        public void PlaySong(int Track)
        {
            throw new NotImplementedException();
        }

        public void PlaySong(LongSong song)
        {
            throw new NotImplementedException();
        }

        public string GetSongInfo()
        {
            //since this is spofify we don't really have much technical info
            return string.Empty;
        }
        private void DownloadCover(SimpleAlbum album, bool firstTry)
        {
            if (album is null)
                return;
            using (System.Net.WebClient cliente = new System.Net.WebClient())
            {
                try
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/covers");
                    if (File.Exists(CoverFileName))
                    {
                        File.Delete(CoverFileName);
                    }
                    if (string.IsNullOrEmpty(album.Id))
                    {
                        File.Delete(CoverFileName);
                        return;
                    }
                    cliente.DownloadFileCompleted += Cliente_DownloadFileCompleted;
                    cliente.DownloadFileAsync(new Uri(album.Images[1].Url), Environment.CurrentDirectory + CoverFileName);
                }
                catch (System.Net.WebException ex)
                {
                    if (firstTry)
                    {
                        Log.Instance.PrintMessage("Couldn't download the album cover, retrying...", MessageType.Warning);
                        DownloadCover(album, false);
                    }
                    else
                    {
                        Log.Instance.PrintMessage("Second try failed.", MessageType.Error);
                        Log.Instance.PrintMessage(ex.Status.ToString(), MessageType.Warning);

                    }
                    File.Delete(CoverFileName);
                }
                catch (IOException)
                {
                    Log.Instance.PrintMessage("Couldn't download the album cover, cannot replace...", MessageType.Error);
                }
            }
        }

        private void Cliente_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            CoverAvailable.Invoke(null, null);
        }
    }
}
