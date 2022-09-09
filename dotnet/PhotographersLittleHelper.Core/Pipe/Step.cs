using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public abstract class Step<I, O> : IStep<I, O>
    {
        public IStepIn<O> NextStep { get; set; }

        public Step() { }

        public async Task<bool> WorkAsync(I input)
        {
            O output = await WorkInternalAsync(input).ConfigureAwait(false);

            if (NextStep != null)
            {
                return await NextStep.WorkAsync(output).ConfigureAwait(false);
            } else
            {
                return true;
            }
        }

        protected abstract Task<O> WorkInternalAsync(I input);
    }
}
