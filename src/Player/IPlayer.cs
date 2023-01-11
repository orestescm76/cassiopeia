using Cassiopeia.src.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cassiopeia.src.Player
{
    public interface IPlayer : IDisposable
    {
        public PlayingState State { get; set; }
        public TimeSpan Position { get; }
        public TimeSpan Duration { get; }
        public bool Shuffle { get; set; }
        public float Volume { get; set; }
        public int PlaylistPointer { get; set; }
        public Playlist Playlist { get; set; }
        /// <summary>
        /// Gets the cover for the playing song
        /// </summary>
        /// <returns></returns>
        public Image GetCover();
        /// <summary>
        /// Returns the name of the playing song
        /// </summary>
        /// <returns>Artist - Title (Album)</returns>
        public string GetSongPlaying();
        /// <summary>
        /// Returns the song info like bitrate
        /// </summary>
        /// <returns></returns>
        public string GetSongInfo();
        public void Init();
        public void Play();
        public void Pause();
        public void Stop();
        /// <summary>
        /// Seeks the song
        /// </summary>
        /// <param name="where">Where to, in local player seconds; in Spotify milliseconds</param>
        public void Seek(int where);
        public void Previous();
        public void Next();
        public void SetShuffle();
        public void HandleDragDrop();
        public void PlaySong(string path);
        public void PlaySong(Song c);
        public void PlaySong(int Track);
        public void PlaySong(LongSong song);
    }
    public enum PlayingState
    {
        Playing,
        Paused,
        Stop
    }
}
