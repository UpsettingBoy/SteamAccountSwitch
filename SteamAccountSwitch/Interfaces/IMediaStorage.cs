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
