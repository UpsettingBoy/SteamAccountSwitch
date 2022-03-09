using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;

using Windows.Graphics.Imaging;

namespace SteamAccountSwitch.Models
{
    public partial class SteamAccountModel: ObservableObject
    {
        [ObservableProperty]
        private string _steamId;
        [ObservableProperty]
        private string _accountName;
        [ObservableProperty]
        private string _displayName;
        [ObservableProperty]
        private bool _savedPassword;
        [ObservableProperty]
        private SoftwareBitmap _avatar;

        // TODO: These 2 should't be here, but I don't know how to
        // bind to elements outside of the DataTeplate / ItemsSource
        // (Views::SteamAccountsView & ViewModels::SteamAccountsViewModel)
        [ObservableProperty]
        private SoftwareBitmapSource _avatarSource;
        [ObservableProperty]
        private double _avatarWidth;
        [ObservableProperty]
        private bool _launching;
    }
}
