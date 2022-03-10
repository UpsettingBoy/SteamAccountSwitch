using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SteamAccountSwitch.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamAccountSwitch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        public SettingsViewModel ViewModel { get; set; }

        public SettingsView()
        {
            this.InitializeComponent();

            ViewModel = new SettingsViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SteamExeBrowseEnabled = string.IsNullOrWhiteSpace(ViewModel.Settings?.SteamExePath);

            // TODO: Only half icon displaying
            //var steamGeometry = SettingsUI.Helpers.GeneralHelper.GetGeometry("SteamIconGeometry");
            //var steamIcon = new PathIcon { Data = steamGeometry, Scale = new Vector3(0.25f) };

            //SteamSettings.Icon = steamIcon;
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.Save();
        }

        private void SteamExePathTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel.Settings.SteamExePath is not null)
            {
                var pathIsNullEmptyWhite = string.IsNullOrWhiteSpace(ViewModel.Settings.SteamExePath);
                var pathNoChange = ViewModel.Settings.SteamExePath.Equals(SteamExePathTb.Text);

                ViewModel.SteamExeBrowseEnabled = !pathIsNullEmptyWhite && !pathNoChange;
            }
            else
            {
                ViewModel.SteamExeBrowseEnabled = false;
            }
        }

        private async void SteamExePathBrowseBt_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();

            var windowHandle = WinUIEx.HwndExtensions.GetActiveWindow();
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, windowHandle);

            filePicker.CommitButtonText = "Select";
            filePicker.FileTypeFilter.Add(".exe");
            
            var file = await filePicker.PickSingleFileAsync();
            if (file is not null)
            {
               ViewModel.Settings.SteamExePath = file.Path;
            }   
        }

        private void ThemeSelectorCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var window = ((App)Application.Current).Window;

            if (window.Content is FrameworkElement fe)
            {
                fe.RequestedTheme = ViewModel.Settings.AppTheme;
            }

            ViewModel.Settings.Save();
        }
    }
}
