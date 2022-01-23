using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

namespace SteamAccountSwitch.Utils
{
    internal static class Cache
    {
        private static ApplicationDataContainer _settings = ApplicationData.Current.LocalSettings;
        private static StorageFolder _cache = ApplicationData.Current.TemporaryFolder;

        public static void AddSetting(string key, object value)
        {
            _settings.Values[key] = value;
        }

        public static T GetSetting<T>(string key)
        {
            try
            {
                return (T)_settings.Values[key];
            }
            catch (Exception)
            { 
                return default;
            }
        }

        public static async Task CacheData(string fileName, IBuffer buffer)
        {
            var cacheFile = await _cache.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBufferAsync(cacheFile, buffer);
        }

        public static async Task<IBuffer> GetCacheData(string filename)
        {
            try
            {
                var cacheFile = await _cache.GetFileAsync(filename);
                return await FileIO.ReadBufferAsync(cacheFile);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}