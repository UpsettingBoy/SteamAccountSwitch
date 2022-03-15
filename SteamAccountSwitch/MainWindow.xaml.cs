using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

using SteamAccountSwitch.Messages;
using SteamAccountSwitch.Views;

using System;
using System.Collections.Generic;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamAccountSwitch
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx, IRecipient<GoToSettingsMessage>
    {
        private string? _prevPage = null;
        
        private readonly Dictionary<string, Type> _viewTranslator = new()
        {
            { "Accounts", typeof(SteamAccountsView) },
            { "Settings", typeof(SettingsView)}
        };

        public MainWindow()
        {
            this.InitializeComponent();

            WeakReferenceMessenger.Default.Register(this);

            Navigate("Accounts");
        }

        public void Receive(GoToSettingsMessage message)
        {
            Navigate("Settings");
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            Navigate((args.InvokedItem as string)!, args.RecommendedNavigationTransitionInfo);
        }

        private void Navigate(string pageName, NavigationTransitionInfo? transitionInfo = null)
        {            
            if (_prevPage is not null && _prevPage.Equals(pageName))
            {
                return;
            }
            
            var isSettings = pageName.Equals("Settings");
            
            if (isSettings)
            {
                NavigationView.SelectedItem = NavigationView.SettingsItem;
            }
            else
            {
                var items = NavigationView.MenuItems.Union(NavigationView.MenuItems);
                var item = items.First(x =>
                {
                    var menuItem = x as NavigationViewItem;
                    var contentString = menuItem?.Content.ToString();
                    if (menuItem is null || contentString is null)
                    {
                        return false;
                    }

                    return contentString.Equals(pageName);

                });

                NavigationView.SelectedItem = item;
            }

            var pageType = _viewTranslator.FirstOrDefault(x => x.Key == pageName).Value;
            var navOptions = new FrameNavigationOptions
            {
                TransitionInfoOverride = transitionInfo?? new SlideNavigationTransitionInfo()
                {
                    Effect = SlideNavigationTransitionEffect.FromBottom
                }
            };

            _prevPage = pageName;
            PageFrame.NavigateToType(pageType, null, navOptions);
        }
    }
}
