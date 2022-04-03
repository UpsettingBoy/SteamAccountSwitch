using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;

using Turbine.Interfaces;
using Turbine.Models;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;

namespace Turbine.ViewModels
{
    public partial class SteamAccountsViewModel : ObservableObject
    {
        private ISteam _steam;

        public ObservableCollection<SteamAccountModel> Accounts { get; set; }

        public SteamAccountsViewModel()
        {
            var container = ((App)Application.Current).Container;

            _steam = container.GetService<ISteam>()!;

            Accounts = new ObservableCollection<SteamAccountModel>();
        }

        public async Task LoadSteamAccounts(double panelWidth, bool forceDownload)
        {
            Accounts.Clear();
            var accounts = (await _steam.GetSteamAccountsAsync(forceDownload)).OrderBy(acc => acc.SteamId).ToList();

            var imageDims = (panelWidth - 160) / Math.Clamp(accounts.Count, 1.0, 3.0);


            foreach (var acc in accounts)
            {
                var validBitmap = SoftwareBitmap.Convert(acc.Avatar, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);

                var image = new SoftwareBitmapSource();
                await image.SetBitmapAsync(validBitmap);

                acc.AvatarSource = image;
                acc.AvatarWidth = (panelWidth - 160) / Math.Clamp(accounts.Count, 1.0, 3.0);
                acc.Launching = false;

                Accounts.Add(acc);
            }
        }

        public async Task LaunchSteamAsync(SteamAccountModel acc, bool offline)
        {
            acc.Launching = true;
            await _steam.LaunchSteamAsync(acc, offline);
            acc.Launching = false;
        }

        public static MenuFlyoutItemTag CreateFlyoutTag(string steamId, bool offline)
        {
            return new MenuFlyoutItemTag() { SteamId = steamId, Offline = offline };
        }

        public static Visibility PasswordToVisibility(bool isSaved)
        {
            return isSaved ? Visibility.Collapsed : Visibility.Visible;
        }
    }


#nullable disable
    public class MenuFlyoutItemTag
    {
        public string SteamId { get; set; }
        public bool Offline { get; set; }
    }
}
