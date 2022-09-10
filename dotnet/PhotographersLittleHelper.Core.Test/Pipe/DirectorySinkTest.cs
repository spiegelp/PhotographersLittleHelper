using PhotographersLittleHelper.Core.Pipe;
using PhotographersLittleHelper.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Test.Pipe
{
    public class DirectorySinkTest
    {
        public DirectorySinkTest() { }

        [Fact]
        public async Task WorkAsync_Ok()
        {
            string directory = @"C:\temp\photos\out";
            string filename = "sakura_1000_series_yukemuri.jpg";

            PhotoService photoService = new();
            PhotoData photoData = await photoService.ReadPhotoAsync(Path.Combine( @"C:\temp\photos\in", filename));

            DirectorySink sink = new() { Directory = directory };
            await sink.WorkAsync(photoData);

            Assert.True(File.Exists(Path.Combine(directory, filename)));
        }
    }
}
