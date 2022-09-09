using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class DirectorySink : ISink<PhotoData>
    {
        public string Directory { get; set; }

        public DirectorySink() { }

        public async Task<bool> WorkAsync(PhotoData input)
        {
            string filename = Path.Combine(Directory, input.Filename);

            await File.WriteAllBytesAsync(filename, input.Data).ConfigureAwait(false);

            return true;
        }
    }
}
