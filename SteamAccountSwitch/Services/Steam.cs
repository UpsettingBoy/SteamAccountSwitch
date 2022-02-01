using Gameloop.Vdf;

using Microsoft.Extensions.Configuration;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;

using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.Web.Http;

namespace SteamAccountSwitch.Utils
{
    

    internal class Steam
    {
        public delegate void UpdateUi(string data);

#if CI
        private static readonly string STEAM_KEY = Environment.GetEnvironmentVariable("STEAM_KEY");
#else
        private static readonly string STEAM_KEY = App.Secrets["STEAM_KEY"];
#endif


        private const string STEAM_LAUNCH = "reg add \"HKCU\\Software\\Valve\\Steam\" /v AutoLoginUser /t REG_SZ /d {0} /f \n reg add \"HKCU\\Software\\Valve\\Steam\" /v RememberPassword /t REG_DWORD /d 1 /f";

        private static SteamWebInterfaceFactory _factory;
        private static SteamUser _userApi;

        static Steam()
        {
            _factory = new SteamWebInterfaceFactory(STEAM_KEY);
            _userApi = _factory.CreateSteamWebInterface<SteamUser>();
    }

        public static async Task GetInstallerLocations(DispatcherQueue dispatcher, UpdateUi update)
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "WHERE.exe",
                Arguments = @"/R C:\ steam.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            });

            process.OutputDataReceived += (_, args) =>
            {
                var item = args.Data;
                if (item != null)
                {
                    dispatcher.TryEnqueue(DispatcherQueuePriority.Low, () => update(args.Data));
                }
            };

            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
        }

        public static async Task CloseSteam()
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Cache.GetSetting<string>("steam_installation"),
                Arguments = "-shutdown",
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            await process.WaitForExitAsync();
        }

        public static async Task LaunchSteam(string accountName)
        {

            if (System.Diagnostics.Process.GetProcessesByName("Steam").Length > 0)
            {
                await CloseSteam();
                await Task.Delay(3000); // Steam closing is too slow. Need some extra time to change registry after Steam is done.
            }

            var userLaunch = string.Format(STEAM_LAUNCH, accountName);
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            { 
                FileName = "CMD.exe",
                Arguments = "/C " + userLaunch,
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            await process.WaitForExitAsync();
            await Task.Delay(3000); // Still needs some extra time to launch properly every time.

            process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Cache.GetSetting<string>("steam_installation"),
                Arguments = "-login",
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            await process.WaitForExitAsync();
        }

        public static IList<(string, string)> GetAccountsIds()
        {
            var steamPath = Cache.GetSetting<string>("steam_installation");
            if (steamPath == null)
            {
                return null;
            }

            var accountsFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(steamPath), "config", "loginusers.vdf");

            var accounts = new List<(string, string)>();
            dynamic accountsParsed = VdfConvert.Deserialize(System.IO.File.ReadAllText(accountsFile));
            
            foreach (var acc in accountsParsed.Value)
            {
                accounts.Add((acc.Key, acc.Value[0].Value.Value));
            }

            return accounts;
        }

        public static async Task<BitmapImage> GetAccountAvatar(string steamId, bool forceDownload = false)
        {
            var imageBuffer = await Cache.GetCacheData(steamId + ".jpeg");

            if (forceDownload || imageBuffer == null)
            {
                var userResponse = await _userApi.GetPlayerSummaryAsync(ulong.Parse(steamId));
                var userData = userResponse.Data;

                using var client = new HttpClient();
                var avatarBuffer = await client.GetBufferAsync(new System.Uri(userData.AvatarFullUrl));

                await Cache.CacheData(steamId + ".jpeg", avatarBuffer);
                imageBuffer = avatarBuffer;
            }

            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(imageBuffer.AsStream().AsRandomAccessStream());

            return bitmap;
        }
    }
}
