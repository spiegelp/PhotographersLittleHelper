using PhotographersLittleHelper.Core.Pipe;
using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Test.Services
{
    public class PhotoServiceTest
    {
        public PhotoServiceTest() { }

        [Fact]
        public async Task ReadPhotoAsync_Ok()
        {
            PhotoService photoService = new();
            PhotoData photoData = await photoService.ReadPhotoAsync(@"C:\temp\photos\in\sakura_1000_series_yukemuri.jpg");

            Assert.NotNull(photoData);
            Assert.NotEmpty(photoData.Data);
            Assert.Equal("sakura_1000_series_yukemuri.jpg", photoData.Filename);
            Assert.Equal(ImageFormat.Jpeg, photoData.Format);
        }

        [Fact]
        public void GetImageFormatForFilename_Ok()
        {
            PhotoService photoService = new();

            Assert.Equal(ImageFormat.Jpeg, photoService.GetImageFormatForFilename("photo.jpg"));
            Assert.Equal(ImageFormat.Jpeg, photoService.GetImageFormatForFilename("photo.jpeg"));
            Assert.Equal(ImageFormat.Png, photoService.GetImageFormatForFilename("photo.png"));
            Assert.Equal(ImageFormat.Gif, photoService.GetImageFormatForFilename("photo.gif"));
            Assert.Throws<ArgumentException>(() => photoService.GetImageFormatForFilename("photo.docx"));
        }
    }
}
