using PhotographersLittleHelper.Core.Pipe;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Services
{
    public class PhotoService
    {
        public ISet<ImageFormat> SupportedImageFormats { get; init; }

        public PhotoService()
        {
            SupportedImageFormats = new HashSet<ImageFormat>
            {
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Gif
            };
        }

        public async Task<PhotoData> ReadPhotoAsync(string filename)
        {
            FileInfo file = new(filename);

            ImageFormat format = GetImageFormatForFilename(filename);
            byte[] data = await File.ReadAllBytesAsync(filename).ConfigureAwait(false);

            return new() { Data = data, Filename = file.Name, Format = format };
        }

        public ImageFormat GetImageFormatForFilename(string filename)
        {
            FileInfo file = new(filename);
            string extension = file.Extension.ToLower();

            return extension switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".gif" => ImageFormat.Gif,
                _ => throw new ArgumentException($"Extension of {file.Name} is not supported.", nameof(filename)),
            };
        }

        public async Task CompressPhotosAsync(string sourceDirectory, string sinkDirectory, int quality)
        {
            await Task.Run(async () =>
            {
                if (!string.IsNullOrWhiteSpace(sourceDirectory) && new DirectoryInfo(sourceDirectory).Exists
                    && !string.IsNullOrWhiteSpace(sinkDirectory) && new DirectoryInfo(sinkDirectory).Exists
                    && quality >= 1 && quality <= 100)
                {
                    DirectorySink sink = new() { Directory = sinkDirectory };
                    PhotoCompressionStep compressionStep = new() { Quality = quality, NextStep = sink };
                    DirectorySource source = new() { Directory = sourceDirectory, NextStep = compressionStep };

                    await Task.Run(async () => await source.WorkAsync().ConfigureAwait(false))
                        .ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }
    }
}
