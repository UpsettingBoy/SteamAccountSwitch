using SteamAccountSwitch.Interfaces;

using System;
using System.IO;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;
using Windows.Storage;

namespace SteamAccountSwitch.Services.Windows
{
    public class WindowsMediaStorageService : IMediaStorage
    {
        private StorageFolder _storageFolder;

        public WindowsMediaStorageService()
        {
            _storageFolder = ApplicationData.Current.TemporaryFolder;
        }

        public async Task<SoftwareBitmap?> LoadBitmapAsync(string key)
        {
            StorageFile file;
            try
            {
                file = await _storageFolder.GetFileAsync(key);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            using var stream = await file.OpenAsync(FileAccessMode.Read);
            if (stream.Size == 0)
            {
                return null;
            }
            
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var tmp = await decoder.GetSoftwareBitmapAsync();

            return tmp;
        }

        public async Task StoreBitmapAsync(string key, SoftwareBitmap bitmap)
        {
            var file = await _storageFolder.CreateFileAsync(key, CreationCollisionOption.OpenIfExists);

            using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

            encoder.SetSoftwareBitmap(bitmap);

            await encoder.FlushAsync();
        }
    }
}
