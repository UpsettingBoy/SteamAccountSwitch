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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamAccountSwitch.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SteamPath.Text = Cache.GetSetting<string>("steam_installation");
        }

        private void SteamPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SteamPath.Text == Cache.GetSetting<string>("steam_installation"))
            {
                SteamSaveBt.IsEnabled = false;
            }
            else
            {
                SteamSaveBt.IsEnabled = true;
            }
        }

        private async void SteamBrowseBt_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Steam installation selector",
                PrimaryButtonText = "Use selected installation",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = new SteamSettingsDialog(),
                XamlRoot = Content.XamlRoot
            };

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                var steamSettings = dialog.Content as SteamSettingsDialog;
                var steamInstallation = steamSettings.SteamInstallPath;

                SteamPath.Text = steamInstallation;
            }
        }

        private void SteamSaveBt_Click(object sender, RoutedEventArgs e)
        {
            Cache.AddSetting("steam_installation", SteamPath.Text);
        }
    }
}
