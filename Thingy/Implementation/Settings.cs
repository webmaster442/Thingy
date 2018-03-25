using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Thingy.API;

namespace Thingy.Implementation
{
    public class Settings : ISettings
    {
        private Dictionary<string, string> _config;
        private readonly ILog _log;

        public Settings(ILog log)
        {
            _log = log;
            Load();
        }

        public bool Exists(string key)
        {
            return _config.ContainsKey(key);
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (_config.ContainsKey(key))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(_config[key]);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                    return defaultValue;
                }
            }
            else
            {
                _log.Warning("Setting not found. Returning default value for: {0} - {1}", key, defaultValue);
                return defaultValue;
            }
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
                _log.Error(ex);
                _config = new Dictionary<string, string>();
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
                _log.Error(ex);
            }
        }

        public void Set<T>(string key, T value)
        {
            if (_config.ContainsKey(key))
            {
                try
                {
                    _log.Info("Modifiying existing {0} key value from {1} to {1}", key, _config[key], value);
                    _config[key] = JsonConvert.SerializeObject(value);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }
            else
            {
                try
                {
                    _log.Info("Creating setting: {0} with value: {1}", key, value);
                    _config.Add(key, JsonConvert.SerializeObject(value));
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }
        }
    }
}
