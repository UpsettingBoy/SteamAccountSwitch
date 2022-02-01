using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SteamAccountSwitch.Utils;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamAccountSwitch.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SteamAccountsPage : Page
    {
        public ObservableCollection<SteamProfile> Accounts { get; set; }

        private bool _loaded = false;

        public SteamAccountsPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            Accounts = new ObservableCollection<SteamProfile>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            while (string.IsNullOrWhiteSpace(Cache.GetSetting<string>("steam_installation")) || !Cache.GetSetting<string>("steam_installation").Contains("steam.exe"))
            {
                var dialog = new ContentDialog
                {
                    Title = "Steam installation selector",
                    PrimaryButtonText = "Use selected installation",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = new SteamSettingsDialog(),
                    XamlRoot = Content.XamlRoot
                };

                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    var steamSettings = dialog.Content as SteamSettingsDialog;
                    Cache.AddSetting("steam_installation", steamSettings.SteamInstallPath);
                }
            }

            if (!_loaded)
            {
                var steamIds = Utils.Steam.GetAccountsIds();
                foreach (var (id, name) in steamIds)
                {
                    var avatar = await Utils.Steam.GetAccountAvatar(id, true);
                    Accounts.Add(new SteamProfile
                    {
                        AccountName = name,
                        Image = avatar
                    });
                }

                _loaded = true;
            }
        }

        private async void AccountsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = (SteamProfile)e.ClickedItem;
            var launchTask = Utils.Steam.LaunchSteam(clicked.AccountName);

            App.Minimize();
            await launchTask;
        }
    }
}
