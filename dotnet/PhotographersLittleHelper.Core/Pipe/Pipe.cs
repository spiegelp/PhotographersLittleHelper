using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public class Pipe<T>
    {
        public ISource<T> Source { get; private set; }

        private Pipe() { }

        public static Pipe<T> Create(ISource<T> source, List<IStep<T, T>> steps, ISink<T> sink)
        {
            IStepIn<T> nextStep = sink;

            if (steps != null && steps.Any())
            {
                for (int i = steps.Count - 1; i >= 0; i--)
                {
                    steps[i].NextStep = nextStep;
                    nextStep = steps[i];
                }
            }

            source.NextStep = nextStep;

            return new() { Source = source };
        }
    }
}
