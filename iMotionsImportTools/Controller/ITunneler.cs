﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMotionsImportTools.iMotionsProtocol;

namespace iMotionsImportTools.Controller
{
    public interface ITunneler
    {

        bool ShouldTunnel { get; set; }

        event EventHandler<Sample> Transport;
    }
}
