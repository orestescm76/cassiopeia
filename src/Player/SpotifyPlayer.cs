using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        public PrivateUser User { get; set; }
        private PrivateUser user;
        bool SpotifyListo = false;
        public bool UserIsPremium { get; private set; }
        bool SpotifySync;
        public FullTrack PlayingSong { get; private set; }
        public string PreviousSpotifyID { get; private set; }
        private readonly Spotify SpotifyAPI = Kernel.Spotify;
        public SpotifyPlayer()
        {
            Init();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void HandleDragDrop()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            try
            {
                user = Kernel.Spotify.GetPrivateUser();
                Log.Instance.PrintMessage("Starting player with Spotify mode, e-mail: " + user.Email, MessageType.Info);
                UserIsPremium = Kernel.Spotify.UserIsPremium();
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

        public void Seek()
        {
            throw new NotImplementedException();
        }

        public void SetShuffle()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            Log.Instance.PrintMessage("Shutting down Spotify", MessageType.Info);


        }

        public string GetSongPlaying()
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Image GetCover()
        {
            throw new NotImplementedException();
        }

        public void PlaySong(string path)
        {
            throw new NotImplementedException();
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
    }
}
