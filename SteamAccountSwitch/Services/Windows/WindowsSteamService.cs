using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

using Microsoft.Extensions.Configuration;
//using Microsoft.UI.Xaml.Media.Imaging;

using SteamAccountSwitch.Interfaces;
using SteamAccountSwitch.Models;

using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;
using Windows.Web.Http;

namespace SteamAccountSwitch.Services.Windows
{
    public class WindowsSteamService : ISteam
    {
        private IConfig _config;
        private IMediaStorage _mediaStorage;

        private SteamUser _userApi;

        private const string STEAM_CHANGE_USER_TWEAK = "reg add \"HKCU\\Software\\Valve\\Steam\" /v AutoLoginUser /t REG_SZ /d {0} /f \n reg add \"HKCU\\Software\\Valve\\Steam\" /v RememberPassword /t REG_DWORD /d 1 /f";

        public WindowsSteamService(IConfig config, IMediaStorage mediaStorage, IConfiguration secrets)
        {
            _config = config;
            _mediaStorage = mediaStorage;

            var factory = new SteamWebInterfaceFactory(secrets["STEAM_KEY"]);
            _userApi = factory.CreateSteamWebInterface<SteamUser>();
        }

        public async Task CloseSteamAsync()
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = _config.GetConfig<string>("SteamExePath")!,
                Arguments = "-shutdown",
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            await process!.WaitForExitAsync();
            await Task.Delay(3000); // Steam closing is too slow. Wait a bit to do another action
        }

        public async Task<IList<SteamAccountModel>> GetSteamAccountsAsync(bool forceDownload)
        {
            var steamPath = _config.GetConfig<string>("SteamExePath");
            
            if (string.IsNullOrWhiteSpace(steamPath))
            {
                return new List<SteamAccountModel>();
            }

            var accountsFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(steamPath)!, "config", "loginusers.vdf");

            var accounts = new List<SteamAccountModel>();
            dynamic accountsParsed = VdfConvert.Deserialize(System.IO.File.ReadAllText(accountsFile));

            foreach (var acc in accountsParsed.Value)
            {
                var steamId = acc.Key as string;

                var savedPasswordInt = 0;
                _ = int.TryParse(acc.Value["RememberPassword"].Value, out savedPasswordInt) ? savedPasswordInt : 0;

                var avatar = await GetAccountAvatarAsync(steamId!, forceDownload);

                accounts.Add(new SteamAccountModel()
                {
                    SteamId = steamId!,
                    AccountName = acc.Value["AccountName"].Value,
                    DisplayName = acc.Value["PersonaName"].Value,
                    SavedPassword = savedPasswordInt == 1,
                    Avatar = avatar!,
                });
            }

            return accounts;
        }

        public async Task LaunchSteamAsync(SteamAccountModel acc, bool offline)
        {
            if (System.Diagnostics.Process.GetProcessesByName("Steam").Length > 0)
            {
                await CloseSteamAsync();
            }

            var userLaunch = string.Format(STEAM_CHANGE_USER_TWEAK, acc.AccountName);
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "CMD.exe",
                Arguments = "/C " + userLaunch,
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            await process!.WaitForExitAsync();
            
            var waitTask = Task.Delay(3000); // Still needs some extra time to launch properly every time.
            
            await SetLaunchMode(acc.SteamId, offline);
            await waitTask;

            process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = _config.GetConfig<string>("SteamExePath"),
                Arguments = "-login",
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            await process!.WaitForExitAsync();
        }

        private async Task SetLaunchMode(string steamId, bool offline)
        {
            var steamPath = _config.GetConfig<string>("SteamExePath");
            var accountsFile = Path.Combine(Path.GetDirectoryName(steamPath)!, "config", "loginusers.vdf");

            dynamic accountsParsed = VdfConvert.Deserialize(File.ReadAllText(accountsFile));
            dynamic selectedAccount = accountsParsed.Value[steamId];

            selectedAccount.WantsOfflineMode = offline ? 1 : 0;
            selectedAccount.SkipOfflineModeWarning = offline ? 1 : 0;

            await File.WriteAllTextAsync(accountsFile, VdfConvert.Serialize(accountsParsed));
        }

        public async Task<SoftwareBitmap?> GetAccountAvatarAsync(string steamId, bool forceDownload)
        {
            var storageKey = steamId + ".jpeg";
            var avatarBitmap = await _mediaStorage.LoadBitmapAsync(storageKey);

            if (forceDownload || avatarBitmap is null)
            {
                var userResponse = await _userApi.GetPlayerSummaryAsync(ulong.Parse(steamId));
                var userData = userResponse.Data;

                using var client = new HttpClient();
                var avatarBuffer = await client.GetBufferAsync(new Uri(userData.AvatarFullUrl));

                if (avatarBuffer is null)
                {
                    return null;
                }
                
                using var randomStream = avatarBuffer.AsStream().AsRandomAccessStream();
                var decoder = await BitmapDecoder.CreateAsync(randomStream);

                avatarBitmap = await decoder.GetSoftwareBitmapAsync();
                await _mediaStorage.StoreBitmapAsync(storageKey, avatarBitmap);
            }

            return avatarBitmap;
        }
    }
}
