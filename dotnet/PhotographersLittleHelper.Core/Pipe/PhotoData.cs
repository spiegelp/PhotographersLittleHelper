using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class PhotoData
    {
        public byte[] Data { get; set; }

        public ImageFormat Format { get; set; }

        public PhotoData() { }
    }
}
