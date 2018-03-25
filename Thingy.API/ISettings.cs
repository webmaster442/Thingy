﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.API
{
    /// <summary>
    /// Flexible Setting strorage
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Set a Setting
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="key">setting name</param>
        /// <param name="value">Setting value</param>
        void Set<T>(string key, T value);
        /// <summary>
        /// Read a setting
        /// </summary>
        /// <typeparam name="T">Type of setting</typeparam>
        /// <param name="key">setting name</param>
        /// <param name="defaultValue">default value, if reading is not possible</param>
        /// <returns></returns>
        T Get<T>(string key, T defaultValue);
        /// <summary>
        /// Checks if a setting is availbe or not
        /// </summary>
        /// <param name="key">setting name</param>
        /// <returns>true, if the setting exists</returns>
        bool Exists(string key);
        /// <summary>
        /// Remove a setting
        /// </summary>
        /// <param name="key">Setting to remove</param>
        void Remove(string key);
        /// <summary>
        /// Save settings to file
        /// </summary>
        void Save();
    }
}
