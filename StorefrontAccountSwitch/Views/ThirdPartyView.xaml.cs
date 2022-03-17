using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using StorefrontAccountSwitch.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace StorefrontAccountSwitch.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThirdPartyView : Page
    {
        public ThirdPartyViewModel ViewModel { get; private set; }

        public ThirdPartyView()
        {
            this.InitializeComponent();

            ViewModel = new ThirdPartyViewModel();
        }

        private async void LicensePreviewer_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadLicensesAsync();
        }
    }
}
