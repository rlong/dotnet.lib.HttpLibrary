﻿// Copyright (c) 2013 Richard Long & HexBeerium
//
// Released under the MIT license ( http://opensource.org/licenses/MIT )
//

using System;
using System.Collections.Generic;
using System.Text;

namespace jsonbroker.library.server.http
{
    public class HttpMethod
    {

        public static readonly HttpMethod GET = new HttpMethod( "GET" );
        public static readonly HttpMethod POST = new HttpMethod( "POST" );
        public static readonly HttpMethod OPTIONS = new HttpMethod( "OPTIONS" );


        ///////////////////////////////////////////////////////////////////////
        // name
        private String _name;

        public String Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        private HttpMethod(String name)
        {
            _name = name;
        }
    }
}