using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class PhotoResizingStep : Step<PhotoData, PhotoData>
    {
        private int m_maxWidth;
        private int m_maxHeight;

        public int MaxWidth
        {
            get
            {
                return m_maxWidth;
            }

            set
            {
                m_maxWidth = value;

                OnPropertyChanged();
            }
        }

        public int MaxHeight
        {
            get
            {
                return m_maxHeight;
            }

            set
            {
                m_maxHeight = value;

                OnPropertyChanged();
            }
        }

        public PhotoResizingStep()
            : base()
        {
            m_maxWidth = 500;
            m_maxHeight = 500;
        }

#pragma warning disable CS1998 // disable warning that async method does not await anything
        protected override async Task<PhotoData> WorkInternalAsync(PhotoData input)
        {
            using MemoryStream msInput = new(input.Data);
            using Bitmap image = new(msInput);

            if ((m_maxWidth> 0 && m_maxHeight > 0)
                && image.Width > m_maxWidth || image.Height > m_maxHeight)
            {
                int width = image.Width;
                int height = image.Height;

                if (image.Width > m_maxWidth)
                {
                    height = (height * m_maxWidth) / width;
                    width = m_maxWidth;
                }

                if (height > m_maxHeight)
                {
                    width = (width * MaxHeight) / height;
                    height = m_maxHeight;
                }

                Rectangle destRectangle = new(0, 0, width, height);

                using Bitmap destImage = new(width, height);
                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using Graphics graphics = Graphics.FromImage(destImage);

                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, destRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

                using MemoryStream msOutput = new();
                destImage.Save(msOutput, input.Format);

                return new() { Data = msOutput.ToArray(), Filename = input.Filename, Format = input.Format };
            }
            else
            {
                return input;
            }
        }
#pragma warning restore CS1998
    }
}
