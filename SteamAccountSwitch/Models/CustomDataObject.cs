using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAccountSwitch.Pages
{
    public class CustomDataObject
    {
        public string AccountName { get; set; }
        public BitmapImage Image { get; set; }

        public CustomDataObject()
        {
        }
    }
}
