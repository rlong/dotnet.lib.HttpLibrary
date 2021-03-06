﻿// Copyright (c) 2013 Richard Long & HexBeerium
//
// Released under the MIT license ( http://opensource.org/licenses/MIT )
//

using System;
using System.Collections.Generic;
using System.Text;
using dotnet.lib.CoreAnnex.exception;
using dotnet.lib.CoreAnnex.log;

namespace dotnet.lib.Http.headers
{
    /// <summary>
    /// as per http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.7
    /// </summary>
    public class MediaType
    {

        private static Log log = Log.getLog(typeof(MediaType));
        ///////////////////////////////////////////////////////////////////////
        // type
        private String _type;

        public String Type
        {
            get { return _type; }
            protected set { _type = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        // subtype
        private String _subtype;

        public String Subtype
        {
            get { return _subtype; }
            protected set { _subtype = value; }
        }


        ///////////////////////////////////////////////////////////////////////
        // parameters
        private Dictionary<String, String> _parameters;

        Dictionary<String, String> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }


        ///////////////////////////////////////////////////////////////////////
        // toString
        private String _toString;



        ///////////////////////////////////////////////////////////////////////
        // 
        private MediaType(String stringValue)
        {
            _toString = stringValue;
            _parameters = new Dictionary<string, string>();
        }

        public override String ToString()
        {
            return _toString;
        }

        public static MediaType buildFromString(String value)
        {
            int indexOfSlash = value.IndexOf("/");
            if (-1 == indexOfSlash)
            {
                BaseException e = new BaseException(typeof(MediaType), "-1 == indexOfSlash; value = {0}", value);
                throw e;
            }

            MediaType answer = new MediaType(value);

            String type = value.Substring(0, indexOfSlash); // int startIndex, int length
            log.debug(type, "type");
            answer._type = type;
            String remainder = value.Substring(indexOfSlash+1);
            log.debug(remainder, "remainder");

            String subtype;
            {
                int indexOfSemiColon = remainder.IndexOf(";");
                if (-1 == indexOfSemiColon)
                {
                    subtype = remainder.Trim();
                }
                else
                {
                    subtype = remainder.Substring(0, indexOfSemiColon); // int startIndex, int length
                    ParametersScanner scanner = new ParametersScanner(indexOfSemiColon, remainder); 
                    String attribute;
                    while (null != (attribute = scanner.NextAttribute()))
                    {
                        log.debug(attribute, "attribute");
                        String attributeValue = scanner.nextValue();
                        log.debug(attributeValue, "attributeValue");
                        answer._parameters[attribute] = attributeValue;
                    }
                }
            }

            log.debug(subtype, "subtype");
            answer._subtype = subtype;

            return answer;

        }

        public String getParamaterValue(String key, String defaultValue)
        {
            if (!_parameters.ContainsKey(key))
            {
                return defaultValue;
            }
            return _parameters[key];
        }

    }
}
