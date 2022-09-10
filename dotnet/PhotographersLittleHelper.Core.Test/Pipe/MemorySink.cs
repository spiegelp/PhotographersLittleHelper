#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

using PhotographersLittleHelper.Core.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Test.Pipe
{
    public class MemorySink<T> : ISink<T>
    {
        public T Data { get; set; }

        public int Count { get; private set; }

        public MemorySink()
        {
            Count = 0;
        }

        public async Task<bool> WorkAsync(T input)
        {
            Count++;

            Data = input;

            return true;
        }
    }
}

#pragma warning restore CS1998
