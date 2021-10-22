﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus
{
    public interface IEventHandlerDisposeWrapper : IDisposable
    {
        IEventHandler EventHandler { get; }
    }
}