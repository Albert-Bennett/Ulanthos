using System.Collections.Generic;
using System.Linq;
using Microsoft.DirectX.DirectSound;
using Ulanthos.Media.Audio;

namespace Ulanthos.Framework
{
    public class AudioManager : UlanComponent
    {
        static Device audioDevice;
        static List<IAudio> currentlyPlaying = new List<IAudio>();
        static List<JPla> playlists = new List<JPla>();

        /// <summary>
        /// Creates a new AudioManager.
        /// </summary>
        /// <param name="game">Game.</param>
        public AudioManager(Game game)
            : base("AudioManager")
        {
            audioDevice = new Device();
            audioDevice.SetCooperativeLevel(game, CooperativeLevel.Normal);
        }

        /// <summary>
        /// Adds a playlist to this.
        /// </summary>
        public static void Add(string filepath)
        {
            JPla playlist = XMLSerializer.Deserialize<JPla>(filepath, ".JPLA");
            playlist.OnVolumeChanged += new VolumeChanged(OnVolumeChanged);

            playlists.Add(playlist);
        }

        static void OnVolumeChanged(JPla playlist)
        {
            playlist.UseGlobalVolume = true;

            string[] keys = new string[playlist.Music.Keys.Count];
            playlist.Music.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Count(); i++)
                for (int j = 0; j < currentlyPlaying.Count; j++)
                    if (keys[i] == currentlyPlaying[j].Name)
                        currentlyPlaying[j].Volume = playlist.GlobalVolume;
        }


        /// <summary>
        /// Plays an audio file.
        /// </summary>
        /// <param name="playListName">The name of the JPla.</param>
        /// <param name="songName">The name of the JSong.</param>
        public static void Play(string JPlaName, string songName)
        {
            for (int i = 0; i < playlists.Count; i++)
                if (playlists[i].Name == JPlaName)
                    if (playlists[i].Music.ContainsKey(songName))
                    {
                        JSong song = new JSong(playlists[i].Music[songName], audioDevice);
                        song.OnEnded += new Ended(OnEnded);
                        currentlyPlaying.Add(song);
                    }
        }

        static void OnEnded(JSong song)
        {
            song.Dispose();
            currentlyPlaying.Remove(song);
        }

        /// <summary>
        /// Sets the volume of multiple playlists.
        /// </summary>
        /// <param name="JPlaNames">The names of the playlists.</param>
        /// <param name="volume">The volume to set.</param>
        public void SetVolume(string[] JPlaNames, int volume)
        {
            int amount = JPlaNames.Length;

            for (int i = 0; i < amount; i++)
                for (int j = 0; j < playlists.Count; j++)
                    if (playlists[j].Name == JPlaNames[i])
                        playlists[j].GlobalVolume = volume;
        }

        /// <summary>
        /// Applies a Jfx to all of the compatible currently playing JSongs.
        /// </summary>
        /// <param name="effect">The Jfx to be applied.</param>
        public static void ApplyEffect(Jfx effect)
        {
            for (int i = 0; i < currentlyPlaying.Count; i++)
                if (currentlyPlaying[i].JfxCompatible)
                    currentlyPlaying[i].ApplyJfx(effect);
        }

        /// <summary>
        /// Stops a JSong.
        /// </summary>
        /// <param name="name">The name of the JSong.</param>
        public static void Stop(string name)
        {
            for (int i = 0; i < currentlyPlaying.Count; i++)
                if (currentlyPlaying[i].Name == name)
                    currentlyPlaying[i].Stop();
        }

        /// <summary>
        /// Pauses or resumes a JSong.
        /// </summary>
        /// <param name="name">The name of the JSong.</param>
        public static void PauseResume(string name)
        {
            for (int i = 0; i < currentlyPlaying.Count; i++)
                if (currentlyPlaying[i].Name == name)
                        currentlyPlaying[i].PauseResume();
        }

        /// <summary>
        /// Stops all currently playing songs.
        /// </summary>
        public static void StopAll()
        {
            for (int i = 0; i < currentlyPlaying.Count; i++)
                currentlyPlaying[i].Dispose();

            currentlyPlaying.Clear();
        }

        /// <summary>
        /// Pauses or resumes all music.
        /// </summary>
        public static void PauseResumeAll()
        {
            for (int i = 0; i < currentlyPlaying.Count; i++)
                currentlyPlaying[i].PauseResume();
        }
    }
}
