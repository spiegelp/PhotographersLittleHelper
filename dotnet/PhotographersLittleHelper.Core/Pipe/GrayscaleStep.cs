using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class GrayscaleStep : Step<PhotoData, PhotoData>
    {
        public GrayscaleStep() : base() { }

#pragma warning disable CS1998 // disable warning that async method does not await anything
        protected override async Task<PhotoData> WorkInternalAsync(PhotoData input)
        {
            using MemoryStream msInput = new(input.Data);
            using Bitmap image = new(msInput);
            using Bitmap destImage = new(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);

                    double grayscale = 0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B;

                    destImage.SetPixel(x, y, Color.FromArgb((int)grayscale, (int)grayscale, (int)grayscale));
                }
            }

            using MemoryStream msOutput = new();
            destImage.Save(msOutput, input.Format);

            return new() { Data = msOutput.ToArray(), Filename = input.Filename, Format = input.Format };
        }
#pragma warning restore CS1998
    }
}
