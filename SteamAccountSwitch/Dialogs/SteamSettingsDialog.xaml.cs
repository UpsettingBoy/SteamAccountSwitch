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
using Windows.Storage.Pickers;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamAccountSwitch.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SteamSettingsDialog : Page
    {
        public string SteamInstallPath { get; private set; }

        public SteamSettingsDialog()
        {
            this.InitializeComponent();
        }

        private async void Executables_Loaded(object sender, RoutedEventArgs e)
        {
            await Utils.Steam.GetInstallerLocations(Executables.DispatcherQueue, (exec) => Executables.Items.Add(exec));
            SearchProgress.IsActive = false;
        }

        private void Executables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SteamInstallPath = (string)e.AddedItems.First();
            }
        }

        private async void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".exe");

            WinRT.Interop.InitializeWithWindow.Initialize(picker, App.WindowHandle);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                InstallationBox.Text = file.Path;
                SteamInstallPath = file.Path;
            }
        }
    }
}
