using CommunityToolkit.Mvvm.ComponentModel;

using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Turbine.ViewModels
{
    public partial class ThirdPartyViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _licensesText;

        public ThirdPartyViewModel()
        {
            _licensesText = string.Empty;
        }

        public async Task LoadLicensesAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(assembly.GetManifestResourceStream("Turbine.LICENSES.md")!);

            LicensesText = await reader.ReadToEndAsync();
        }
    }
}
