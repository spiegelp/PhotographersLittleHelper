using NuniToolbox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public abstract class Source<T> : ModelObject, ISource<T>
    {
        public IStepIn<T> NextStep { get; set; }

        public abstract Task WorkAsync();
    }
}
