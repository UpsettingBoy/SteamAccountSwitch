using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using StorefrontAccountSwitch.Messages;
using StorefrontAccountSwitch.ViewModels;

using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace StorefrontAccountSwitch.Views
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
            var window = ((App)Application.Current).Window;
            var filePicker = WinUIEx.WindowExtensions.CreateOpenFilePicker(window);

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
            WeakReferenceMessenger.Default.Send(new UpdateAppThemeMessage(ViewModel.Settings.AppTheme));

            ViewModel.Settings.Save();
        }
    }
}
