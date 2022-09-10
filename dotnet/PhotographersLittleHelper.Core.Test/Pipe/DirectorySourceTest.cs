using PhotographersLittleHelper.Core.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Test.Pipe
{
    public class DirectorySourceTest
    {
        public DirectorySourceTest() { }

        [Fact]
        public async Task WorkAsync_Ok()
        {
            MemorySink<PhotoData> sink = new();
            DirectorySource source = new() { Directory = @"C:\temp\photos\in", NextStep = sink };
            await source.WorkAsync();

            Assert.True(sink.Count > 0);
        }
    }
}
