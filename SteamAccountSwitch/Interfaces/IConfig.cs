using System.Threading.Tasks;

namespace SteamAccountSwitch.Interfaces
{
    public interface IConfig
    {
        int Count { get; }

        T? GetConfig<T>(string key);
        T? InitConfig<T>(string key, object? value);
        void SetConfig(string key, object? value);
        Task ClearConfigAsync();
    }

}
