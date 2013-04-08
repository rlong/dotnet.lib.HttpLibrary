﻿// Copyright (c) 2013 Richard Long & HexBeerium
//
// Released under the MIT license ( http://opensource.org/licenses/MIT )
//

using System;
using System.Collections.Generic;
using System.Text;
using jsonbroker.library.common.http.headers;

namespace jsonbroker.library.common.http.multi_part
{
    public interface MultiPartHandler
    {
        PartHandler FoundPartDelimiter();

        void HandleException(Exception e);

        void FoundCloseDelimiter();
    }
}
