using StorefrontAccountSwitch.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorefrontAccountSwitch.Interfaces
{
    public interface ISteam
    {
        Task CloseSteamAsync();

        Task<IList<SteamAccountModel>> GetSteamAccountsAsync(bool forceDownload);
        Task LaunchSteamAsync(SteamAccountModel acc, bool offline);
    }
}
