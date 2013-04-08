﻿// Copyright (c) 2013 Richard Long & HexBeerium
//
// Released under the MIT license ( http://opensource.org/licenses/MIT )
//

using System;
using System.Collections.Generic;
using System.Text;
using jsonbroker.library.common.json;

namespace jsonbroker.library.common.security
{
    public class SecurityConfiguration_Generated
    {
        /////////////////////////////////////////////////////////
        // usersRealm
        private String _localRealm;

        public String LocalRealm
        {
            get { return _localRealm; }
            set { _localRealm = value; }
        }

        /////////////////////////////////////////////////////////
        public SecurityConfiguration_Generated()
        {
        }

        public SecurityConfiguration_Generated(JsonObject values)
        {
            _localRealm = values.getString("localRealm");
        }

        public virtual JsonObject ToJsonObject()
        {
            JsonObject answer = new JsonObject();

            answer.put("localRealm", _localRealm);

            return answer;
        }

    }
}
