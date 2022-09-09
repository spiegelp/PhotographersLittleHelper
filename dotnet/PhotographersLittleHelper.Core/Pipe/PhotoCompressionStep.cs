﻿using System;
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
        public int Quality { get; set; }

        public PhotoCompressionStep()
            : base()
        {
            Quality = 90;
        }

        protected override PhotoData WorkInternal(PhotoData input)
        {
            using MemoryStream msInput = new(input.Data);
            Bitmap image = new(msInput);

            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParams = new(1);
            encoderParams.Param[0] = new(encoder, Quality);

            ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().First(x => x.FormatID == input.Format.Guid);

            using MemoryStream msOutput = new();
            image.Save(msOutput, imageCodec, encoderParams);

            return new() { Data = msOutput.ToArray(), Format = input.Format };
        }
    }
}
