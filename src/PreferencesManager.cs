using System;
using System.IO;
using Maquina.Content;

namespace Maquina
{
    public class PreferencesManager : IDisposable
    {
        public PreferencesManager(string fileName = "preferences.xml")
        {
            FileName = fileName;
        }

        private PreferencesManifest _manifest;

        public void Reset()
        {
            _manifest.Reset();
        }

        public bool GetBoolean(string name, bool defaultValue = default(bool))
        {
            return _manifest.GetPreference(_manifest.BooleanPropertySet, name, defaultValue);
        }
        public int GetInt32(string name, int defaultValue = default(int))
        {
            return _manifest.GetPreference(_manifest.Int32PropertySet, name, defaultValue);
        }
        public float GetFloat(string name, float defaultValue = default(float))
        {
            return _manifest.GetPreference(_manifest.FloatPropertySet, name, defaultValue);
        }
        public string GetString(string name, string defaultValue = default(string))
        {
            return _manifest.GetPreference(_manifest.StringPropertySet, name, defaultValue);
        }

        public void SetBoolean(string name, bool value)
        {
            _manifest.SetPreference(_manifest.BooleanPropertySet, name, value);
        }
        public void SetInt32(string name, int value)
        {
            _manifest.SetPreference(_manifest.Int32PropertySet, name, value);
        }
        public void SetFloat(string name, float value)
        {
            _manifest.SetPreference(_manifest.FloatPropertySet, name, value);
        }
        public void SetString(string name, string value)
        {
            _manifest.SetPreference(_manifest.StringPropertySet, name, value);
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                if (File.Exists(_fileName))
                {
                    _manifest = XmlHelper.Load<PreferencesManifest>(value);
                }

                // Fallback: use default document if preferences don't exist or due to something else
                if (_manifest == null)
                {
                    _manifest = new PreferencesManifest();
#if LOG_ENABLED
                    LogManager.Info(0, "Using default preferences file.");
#endif
                }
            }
        }

        public void Save()
        {
            XmlHelper.Save(_manifest, _fileName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Save();
                _manifest = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
