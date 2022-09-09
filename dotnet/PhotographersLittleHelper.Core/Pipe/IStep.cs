using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public interface IStep<I, O> : IStepIn<I>, IStepOut<O>
    {
        IStepIn<O>? NextStep { get; set; }
    }
}
