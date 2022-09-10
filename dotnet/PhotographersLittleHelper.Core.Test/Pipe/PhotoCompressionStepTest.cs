using PhotographersLittleHelper.Core.Pipe;
using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Test.Pipe
{
    public class PhotoCompressionStepTest
    {
        public PhotoCompressionStepTest() { }

        [Fact]
        public async Task WorkAsync_Ok()
        {
            PhotoService photoService = new();
            PhotoData photoData = await photoService.ReadPhotoAsync(@"C:\temp\photos\in\sakura_1000_series_yukemuri.jpg");

            MemorySink<PhotoData> sink = new();
            PhotoCompressionStep step = new() { Quality = 50, NextStep = sink };
            bool success = await step.WorkAsync(photoData);

            Assert.True(success);
            Assert.Equal("sakura_1000_series_yukemuri.jpg", sink.Data.Filename);
            Assert.True(photoData.Data.Length > sink.Data.Data.Length);
        }
    }
}
