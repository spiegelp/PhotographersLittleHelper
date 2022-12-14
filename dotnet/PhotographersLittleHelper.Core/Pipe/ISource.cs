using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public interface ISource<T> : IStepOut<T>
    {
        IStepIn<T> NextStep { get; set; }

        Task WorkAsync();
    }
}
