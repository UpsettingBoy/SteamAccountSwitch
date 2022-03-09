using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SteamAccountSwitch.Messages;
using SteamAccountSwitch.Models;
using SteamAccountSwitch.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamAccountSwitch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SteamAccountsView : Page
    {
        public SteamAccountsViewModel ViewModel { get; set; }

        public SteamAccountsView()
        {
            this.InitializeComponent();
            ViewModel = new SteamAccountsViewModel();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // When navigating different pages, the loading of the next one
            // happens before the unloading of the previous one (nice!), so
            // because saving the app configuration relies on the unloading
            // of the settings page, incorrect data is loaded here, hence
            // the neccessity of this Task.Delay with smaller time possible
            //
            //  TODO: change something or wait
            // https://github.com/microsoft/microsoft-ui-xaml/issues/6794
            await Task.Delay(300);

            await UpdateViewModelAndPage(false);
        }

        private async Task UpdateViewModelAndPage(bool forceDownload)
        {
            await ViewModel.LoadSteamAccounts(this.ActualWidth, forceDownload);

            LoadingRing.IsActive = false;
            if (ViewModel.Accounts.Count == 0)
            {
                InformationText.Visibility = Visibility.Visible;
            }
            else
            {
                ClearCacheBt.Visibility = Visibility.Visible;
            }
        }

        private async void AccountsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var account = e.ClickedItem as SteamAccountModel;
            await ViewModel.LaunchSteamAsync(account!, false);
        }

        private void SettingsHyperlink_Click(Microsoft.UI.Xaml.Documents.Hyperlink sender, Microsoft.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            WeakReferenceMessenger.Default.Send(new GoToSettingsMessage(true));
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var clickedItem = sender as FrameworkElement;
            var flyout = FlyoutBase.GetAttachedFlyout(clickedItem);

            flyout.ShowAt(clickedItem, new FlyoutShowOptions()
            {
                Position = e.GetPosition(clickedItem),
            });
        }

        private async void MenuFlyoutItem_Click_Steam(object sender, RoutedEventArgs e)
        {            
            var tag = ((sender as MenuFlyoutItem)!.Tag) as MenuFlyoutItemTag;
            var account = ViewModel.Accounts.First(x => x.SteamId.Equals(tag!.SteamId));

            await ViewModel.LaunchSteamAsync(account, tag!.Offline);
        }

        private async void ClearCacheBt_Click(object sender, RoutedEventArgs e)
        {
            await UpdateViewModelAndPage(true);
        }
    }
}
