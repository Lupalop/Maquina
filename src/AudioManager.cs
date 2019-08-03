using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public class AudioManager
    {
        public void PlaySong(string songName, bool isRepeating = true)
        {
            if (Global.BGM.ContainsKey(songName))
            {
                Song song = Global.BGM[songName];
                if (MediaPlayer.Queue.ActiveSong != song)
                {
                    MediaPlayer.Play(song);
                    MediaPlayer.IsRepeating = isRepeating;
                }
            }
#if LOG_ENABLED
            else
            {
                LogManager.Warn(0, string.Format("Unable to play song '{0}' because it doesn't exist.", songName));
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
