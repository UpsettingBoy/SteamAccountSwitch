using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

using StorefrontAccountSwitch.Interfaces;
using StorefrontAccountSwitch.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StorefrontAccountSwitch.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private SettingsModel _settings;
        [ObservableProperty]
        private bool _steamExeBrowseEnabled;

        public static readonly Dictionary<ElementTheme, string> AppThemeTranslator = new()
        {
            { ElementTheme.Dark, "Dark" },
            { ElementTheme.Light, "Light" },
            { ElementTheme.Default, "Windows default" },
        };

        public SettingsViewModel()
        {
            var container = ((App)Application.Current).Container;

            _settings = new SettingsModel(container.GetService<IConfig>()!);
            _steamExeBrowseEnabled = false;
        }
    }

    public class ElementThemeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var themeElement = (ElementTheme)value;
            return SettingsViewModel.AppThemeTranslator.FirstOrDefault(x => x.Key == themeElement).Value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var themeString = value as string;
            return SettingsViewModel.AppThemeTranslator.FirstOrDefault(x => x.Value == themeString).Key;
        }
    }
}