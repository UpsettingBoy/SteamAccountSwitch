using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;

using SteamAccountSwitch.Interfaces;
using SteamAccountSwitch.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;

namespace SteamAccountSwitch.ViewModels
{
    public partial class SteamAccountsViewModel: ObservableObject
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

            foreach (var acc in accounts)
            {
                //var validBitmap = SoftwareBitmap.Convert(acc.Avatar, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
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
            //await Task.Delay(10000);
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

    public class MenuFlyoutItemTag
    {
        public string SteamId{ get; set; }
        public bool Offline { get; set; }
    }
}
