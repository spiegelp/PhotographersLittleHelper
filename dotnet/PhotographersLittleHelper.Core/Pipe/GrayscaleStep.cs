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
            using Bitmap convertedImage = ChangePixelFormat(image, PixelFormat.Format24bppRgb);

            /*using Bitmap destImage = new(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);

                    double grayscale = 0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B;

                    destImage.SetPixel(x, y, Color.FromArgb((int)grayscale, (int)grayscale, (int)grayscale));
                }
            }*/

            // manipulating the bytes directly is much faster than SetPixel()

            Rectangle rectangle = new(0, 0, convertedImage.Width, convertedImage.Height);

            BitmapData bmpData = convertedImage.LockBits(rectangle, ImageLockMode.ReadWrite, convertedImage.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * convertedImage.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int index = 0; index < rgbValues.Length; index = index + 3)
            {
                byte grayscale = (byte)(0.3 * rgbValues[index + 2] + 0.59 * rgbValues[index + 1] + 0.11 * rgbValues[index]);

                rgbValues[index] = grayscale;
                rgbValues[index + 1] = grayscale;
                rgbValues[index + 2] = grayscale;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            convertedImage.UnlockBits(bmpData);

            using MemoryStream msOutput = new();
            convertedImage.Save(msOutput, input.Format);

            return new() { Data = msOutput.ToArray(), Filename = input.Filename, Format = input.Format };
        }
#pragma warning restore CS1998

        private Bitmap ChangePixelFormat(Bitmap image, PixelFormat pixelFormat)
        {
            Bitmap destImage = new(image.Width, image.Height, pixelFormat);
            using Graphics graphics = Graphics.FromImage(destImage);
            graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));

            return destImage;
        }
    }
}
