using PhotographersLittleHelper.Core.Pipe;
using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Test.Pipe
{
    public class PhotoResizingStepTest
    {
        public PhotoResizingStepTest() { }

        [Fact]
        public async Task WorkAsync_Ok()
        {
            PhotoService photoService = new();
            PhotoData photoData = await photoService.ReadPhotoAsync(@"C:\temp\photos\in\sakura_1000_series_yukemuri.jpg");

            MemorySink<PhotoData> sink = new();
            PhotoResizingStep step = new() { MaxWidth = 500, MaxHeight = 500, NextStep = sink };
            bool success = await step.WorkAsync(photoData);

            Assert.True(success);
            Assert.Equal("sakura_1000_series_yukemuri.jpg", sink.Data.Filename);
            Assert.True(photoData.Data.Length > sink.Data.Data.Length);

            using MemoryStream msInput = new(sink.Data.Data);
            using Bitmap image = new(msInput);

            Assert.Equal(500, image.Width);
            Assert.Equal(375, image.Height);
        }
    }
}
