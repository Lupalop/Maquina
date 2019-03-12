using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina.Resources
{
    public class AudioManager
    {
        public AudioManager()
        {
            this.Songs = new Dictionary<string,Song>();
        }

        // Properties
        public Dictionary<string, Song> Songs { get; set; }

        public void PlaySong(string songName, bool isRepeating = true)
        {
            if (Songs.ContainsKey(songName))
            {
                Song song = Songs[songName];
                if (MediaPlayer.Queue.ActiveSong != song)
                {
                    MediaPlayer.Play(song);
                    MediaPlayer.IsRepeating = isRepeating;
                }
            }
#if HAS_CONSOLE && LOG_GENERAL
            else
            {
                Console.WriteLine(String.Format("SceneManager: Song '{0}' not found!", songName));
            }
#endif
        }

        public void PlaySong(string songName)
        {
            PlaySong(songName, true);
        }

        private int musicVolume;
        public int MusicVolume
        {
            get
            {
                return musicVolume;
            }
            set
            {
                MediaPlayer.Volume = value;
                musicVolume = value;
            }
        }

        private float soundVolume;
        public float SoundVolume
        {
            get
            {
                return soundVolume;
            }
            set
            {
                SoundEffect.MasterVolume = value;
                soundVolume = value;
            }
        }

        private bool isMuted;
        public bool IsMuted
        {
            get
            {
                return isMuted;
            }
            set
            {
                if (value)
                {
                    MediaPlayer.IsMuted = true;
                    SoundEffect.MasterVolume = 0;
                }
                else
                {
                    MediaPlayer.IsMuted = false;
                    SoundEffect.MasterVolume = 1;
                }
                isMuted = value;
            }
        }
        public void ToggleMute()
        {
            IsMuted = !IsMuted;
        }
    }
}
