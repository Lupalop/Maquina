using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public class AudioManager : IDisposable
    {
        public AudioManager()
        {
            // Audio
            SoundVolume = Application.Preferences.GetFloat("app.audio.sound", 1f);
            MusicVolume = Application.Preferences.GetInt32("app.audio.music", 255);
            IsMuted = Application.Preferences.GetBoolean("app.audio.mastermuted", false);
        }

        public void PlaySong(string songName, bool isRepeating)
        {
            Song song = (Song)ContentFactory.TryGetResource(songName);
            if (song != null)
            {
                if (MediaPlayer.Queue.ActiveSong != song)
                {
                    MediaPlayer.Play(song);
                    MediaPlayer.IsRepeating = isRepeating;
                }
            }
#if MGE_LOGGING
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

        private int _musicVolume;
        public int MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = value;
                MediaPlayer.Volume = value;
            }
        }

        private float _soundVolume;
        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = value;
                SoundEffect.MasterVolume = value;
            }
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                _isMuted = value;
                MediaPlayer.IsMuted = value;
                SoundEffect.MasterVolume = (value) ? 0 : 1;
            }
        }
        public void ToggleMute()
        {
            IsMuted = !IsMuted;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.Preferences.SetBoolean("app.audio.mastermuted", IsMuted);
                Application.Preferences.SetFloat("app.audio.sound", SoundVolume);
                Application.Preferences.SetInt32("app.audio.music", MusicVolume);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
