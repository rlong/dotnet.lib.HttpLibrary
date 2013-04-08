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
    public interface PartHandler
    {

        void HandleHeader(String name, String value);
        void HandleBytes(byte[] bytes, int offset, int length);

        void HandleException(Exception e);

        void PartCompleted();
    }
}
