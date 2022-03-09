using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;

namespace SteamAccountSwitch.Interfaces
{
    public interface IMediaStorage
    {
        Task StoreBitmapAsync(string key, SoftwareBitmap bitmap);
        Task<SoftwareBitmap?> LoadBitmapAsync(string key);
    }
}
