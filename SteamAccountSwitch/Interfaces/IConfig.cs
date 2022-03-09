using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitch.Interfaces
{
    public interface IConfig
    {
        int Count { get; }

        T? GetConfig<T>(string key);
        void SetConfig(string key, object? value);
        Task ClearConfigAsync();
    }

}
