using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public abstract class Step<I, O> : IStep<I, O>
    {
        public IStepIn<O>? NextStep { get; set; }

        public Step() { }

        public bool Work(I input)
        {
            O output = WorkInternal(input);

            return NextStep?.Work(output) ?? false;
        }

        protected abstract O WorkInternal(I input);
    }
}
