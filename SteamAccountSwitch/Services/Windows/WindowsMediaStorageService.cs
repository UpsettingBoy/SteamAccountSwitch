using SteamAccountSwitch.Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Graphics.Imaging;
using Windows.Storage;

namespace SteamAccountSwitch.Services.Windows
{
    public class WindowsMediaStorageService : IMediaStorage
    {
        private StorageFolder storageFolder;

        public WindowsMediaStorageService()
        {
            storageFolder = ApplicationData.Current.TemporaryFolder;
        }

        public async Task<SoftwareBitmap?> LoadBitmapAsync(string key)
        {
            StorageFile file;
            try
            {
                file = await storageFolder.GetFileAsync(key);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            using var stream = await file.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);

            return await decoder.GetSoftwareBitmapAsync();
        }

        public async Task StoreBitmapAsync(string key, SoftwareBitmap bitmap)
        {
            var file = await storageFolder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);

            using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);

            encoder.SetSoftwareBitmap(bitmap);

            await encoder.FlushAsync();
        }
    }
}
