using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Thingy.API;

namespace Thingy.Implementation
{
    public sealed class Settings : ISettings
    {
        private readonly ILog _log;
        private Dictionary<string, string> _config;

        private void FirePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Load()
        {
            try
            {
                var name = Paths.Resolve(Paths.ConfigPath);
                if (!File.Exists(name))
                {
                    _log.Warning("Config file doesn't exist: {0}", name);
                    _config = new Dictionary<string, string>();
                    return;
                }
                _config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(name));

            }
            catch (Exception ex)
            {
                _log.Exception(ex);
                _config = new Dictionary<string, string>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Settings(ILog log)
        {
            _log = log;
            Load();
        }
        public object this[string key, object defaultValue]
        {
            get => Get(key, defaultValue);
            set => Set(key, value);
        }

        public bool Exists(string key)
        {
            return _config.ContainsKey(key);
        }

        public T Get<T>(string key, T defaultValue)
        {
            _log.Info("Reading setting: {0}...", key);
            if (_config.ContainsKey(key))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(_config[key]);
                }
                catch (Exception ex)
                {
                    _log.Exception(ex);
                    return defaultValue;
                }
            }
            else
            {
                _log.Warning("Setting not found. Returning default value for: {0} - {1}", key, defaultValue);
                Set(key, defaultValue);
                return defaultValue;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            _log.Info("Enumerating settings...");
            return _config.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            _log.Info("Enumerating settings...");
            return _config.GetEnumerator();
        }

        public void Remove(string key)
        {
            if (_config.ContainsKey(key))
            {
                _config.Remove(key);
                _log.Info("Setting removed: {0}", key);
            }
            else
            {
                _log.Warning("Tried to remove non exiting key: {0}", key);
            }
        }
        public void Save()
        {
            try
            {
                var name = Paths.Resolve(Paths.ConfigPath);
                _log.Info("Writing config to: {0}", name);
                if (File.Exists(name))
                    File.Move(name, name + ".old");

                using (var file = File.CreateText(name))
                {
                    var content = JsonConvert.SerializeObject(_config);
                    file.Write(content);
                }

                if (File.Exists(name + ".old"))
                    File.Delete(name + ".old");
            }
            catch (Exception ex)
            {
                _log.Exception(ex);
            }
        }

        public void Set<T>(string key, T value)
        {
            if (_config.ContainsKey(key))
            {
                try
                {
                    _log.Info("Modifiying existing {0} key value from {1} to {2}", key, _config[key], value);
                    _config[key] = JsonConvert.SerializeObject(value);
                    FirePropertyChanged(key);
                }
                catch (Exception ex)
                {
                    _log.Exception(ex);
                }
            }
            else
            {
                try
                {
                    _log.Info("Creating setting: {0} with value: {1}", key, value);
                    _config.Add(key, JsonConvert.SerializeObject(value));
                    FirePropertyChanged(key);
                }
                catch (Exception ex)
                {
                    _log.Exception(ex);
                }
            }
        }
    }
}
