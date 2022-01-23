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
        public ObservableCollection<CustomDataObject> Accounts { get; set; }

        public SteamAccountsPage()
        {
            this.InitializeComponent();
            Accounts = new ObservableCollection<CustomDataObject>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            while (Cache.GetSetting<string>("steam_installation") == null)
            {
                var dialog = new ContentDialog
                {
                    Title = "Steam installation selector",
                    PrimaryButtonText = "Use this installation",
                    DefaultButton = ContentDialogButton.Primary,
                    Content = new SteamSettingsDialog(),
                    XamlRoot = Content.XamlRoot
                };

                await dialog.ShowAsync();
            }

            var steamIds = Utils.Steam.GetAccountsIds();
            foreach (var (id, name) in steamIds)
            {
                var avatar = await Utils.Steam.GetAccountAvatar(id, true);
                Accounts.Add(new CustomDataObject
                {
                    AccountName = name,
                    Image = avatar
                });
            }
        }

        private async void AccountsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var handle = new Vanara.PInvoke.HWND(App.WindowHandle);
            Vanara.PInvoke.User32.ShowWindow(handle, Vanara.PInvoke.ShowWindowCommand.SW_MINIMIZE);

            var clicked = (CustomDataObject)e.ClickedItem;
            await Utils.Steam.LaunchSteam(clicked.AccountName);
        }
    }
}
