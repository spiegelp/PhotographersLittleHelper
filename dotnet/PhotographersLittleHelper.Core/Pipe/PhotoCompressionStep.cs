using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class PhotoCompressionStep : Step<PhotoData, PhotoData>
    {
        public const int MinQuality = 1;
        public const int MaxQuality = 100;

        private int m_quality;

        public int Quality
        {
            get
            {
                return m_quality;
            }

            set
            {
                m_quality = value;

                OnPropertyChanged();
            }
        }

        public PhotoCompressionStep()
            : base()
        {
            Quality = 90;
        }

#pragma warning disable CS1998 // disable warning that async method does not await anything
        protected override async Task<PhotoData> WorkInternalAsync(PhotoData input)
        {
            using MemoryStream msInput = new(input.Data);
            using Bitmap image = new(msInput);

            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParams = new(1);
            encoderParams.Param[0] = new(encoder, Quality);

            ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().First(x => x.FormatID == input.Format.Guid);

            using MemoryStream msOutput = new();
            image.Save(msOutput, imageCodec, encoderParams);

            return new() { Data = msOutput.ToArray(), Filename = input.Filename, Format = input.Format };
        }
#pragma warning restore CS1998
    }
}
