using Turbine.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Turbine.Interfaces
{
    public interface ISteam
    {
        Task CloseSteamAsync();

        Task<IList<SteamAccountModel>> GetSteamAccountsAsync(bool forceDownload);
        Task LaunchSteamAsync(SteamAccountModel acc, bool offline);
    }
}
