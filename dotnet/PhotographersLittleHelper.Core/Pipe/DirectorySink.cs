using NuniToolbox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class DirectorySink : ModelObject, ISink<PhotoData>
    {
        private string m_directoy;

        public string Directory
        {
            get
            {
                return m_directoy;
            }

            set
            {
                m_directoy = value;

                OnPropertyChanged();
            }
        }

        public DirectorySink() { }

        public async Task<bool> WorkAsync(PhotoData input)
        {
            string filename = Path.Combine(Directory, input.Filename);

            await File.WriteAllBytesAsync(filename, input.Data).ConfigureAwait(false);

            return true;
        }
    }
}
