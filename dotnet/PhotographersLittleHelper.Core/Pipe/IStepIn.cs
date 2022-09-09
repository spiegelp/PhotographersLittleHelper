﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotographersLittleHelper.Core.Pipe
{
    public interface IStepIn<T>
    {
        Task<bool> WorkAsync(T input);
    }
}
