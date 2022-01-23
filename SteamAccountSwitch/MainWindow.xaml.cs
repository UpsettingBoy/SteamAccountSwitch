using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

using SteamAccountSwitch.Pages;
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

namespace SteamAccountSwitch
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>
        {
            { "steam_accounts", typeof(SteamAccountsPage) },
            { "settings", typeof(SettingsPage) }
        };

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NavMenu_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string tag;
            if (args.IsSettingsInvoked)
            {
                tag = "settings";
            }
            else
            {
                tag = args.InvokedItemContainer.Tag.ToString();
            }

            NavView_Navigate(tag, args.RecommendedNavigationTransitionInfo);
        }

        private void NavMenu_Loaded(object sender, RoutedEventArgs e)
        {
            NavMenu.SelectedItem = NavMenu.MenuItems[0];
            NavView_Navigate("steam_accounts", new EntranceNavigationTransitionInfo());
        }

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type page = null;
            var item = _pages.FirstOrDefault(p => p.Key.Equals(navItemTag));
            page = item.Value;
            
            var preNavPageType = NavMenuContent.CurrentSourcePageType;

            
            if (page is not null && !Type.Equals(preNavPageType, page))
            {
                NavMenuContent.Navigate(page, null, transitionInfo);
            }
        }
    }
}
