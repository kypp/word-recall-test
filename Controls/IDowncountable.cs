﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls
{
    public interface IDowncountable
    {
        TimeSpan Countdown { get; set; }
    }
}
