﻿// Copyright (c) 2013 Richard Long & HexBeerium
//
// Released under the MIT license ( http://opensource.org/licenses/MIT )
//

using System;
using System.Collections.Generic;
using System.Text;
using dotnet.lib.CoreAnnex.log;
using dotnet.lib.Http.json_broker.test;
using dotnet.lib.Http.headers.request;
using dotnet.lib.Http.headers;
using dotnet.lib.Http;
using dotnet.lib.Http.server;
using dotnet.lib.CoreAnnex.exception;

namespace dotnet.lib.Http.authentication.server
{
    public class AuthRequestHandler : RequestHandler
    {

        private static Log log = Log.getLog(typeof(AuthRequestHandler));

        private static readonly String REQUEST_URI = "/_dynamic_/auth";


 
        /////////////////////////////////////////////////////////
        private Dictionary<String, RequestHandler> _processors;

        /////////////////////////////////////////////////////////
        // securityManager
        private HttpSecurityManager _securityManager;

        protected HttpSecurityManager SecurityManager
        {
            get { return _securityManager; }
            set { _securityManager = value; }
        }


        public AuthRequestHandler(HttpSecurityManager securityManager)
        {
            _processors = new Dictionary<String, RequestHandler>();
            _securityManager = securityManager;
        }

        public void AddRequestHandler(RequestHandler processor)
        {
            String requestUri = REQUEST_URI + processor.getProcessorUri();
            log.debug(requestUri, "requestUri");
            _processors[requestUri] = processor;
        }


        private RequestHandler GetRequestHandler(String requestUri)
        {
            log.debug(requestUri, "requestUri");

            int indexOfQuestionMark = requestUri.IndexOf('?');
            if (-1 != indexOfQuestionMark)
            {
                requestUri = requestUri.Substring(0, indexOfQuestionMark);
            }

            if (!_processors.ContainsKey(requestUri))
            {
                return null;
            }

            RequestHandler answer = _processors[requestUri];
            return answer;

        }


        public String getProcessorUri()
        {
            return REQUEST_URI;
        }



        private Authorization getAuthorizationRequestHeader(HttpRequest request)
        {

            Authorization answer = null;


            String authorization = request.getHttpHeader("authorization");
            if (null == authorization)
            {
                log.error("null == authorization");
                throw HttpErrorHelper.unauthorized401FromOriginator(this );
            }

            answer = Authorization.buildFromString(authorization);

            if (!"auth".Equals(answer.qop))
            {
                log.errorFormat("!\"auth\".Equals(answer.qop); answer.qop = '{0}'", answer.qop);
                throw HttpErrorHelper.unauthorized401FromOriginator(this);
            }

            return answer;
        }


        public HttpResponse processRequest(HttpRequest request)
        {

            RequestHandler httpProcessor;
            {
                String requestUri = request.RequestUri;
                httpProcessor = GetRequestHandler(requestUri);
                if (null == httpProcessor)
                {
                    log.errorFormat("null == httpProcessor; requestUri = {0}", requestUri);
                    throw HttpErrorHelper.notFound404FromOriginator(this);
                }
            }

            HttpResponse answer = null;
            Authorization authorization = null;

            try
            {
                authorization = getAuthorizationRequestHeader(request);
                _securityManager.authenticateRequest(request.Method.Name, authorization);

                answer = httpProcessor.processRequest(request);
                return answer;
            }
            catch (BaseException e)
            {

                if ( HttpStatus.ErrorDomain.UNAUTHORIZED_401.Equals(e.ErrorDomain))
                {
                    log.warn(e.Message);
                }
                else
                {
                    log.error(e);
                }

                answer = HttpErrorHelper.toHttpResponse(e);
                return answer;
            }
            catch (Exception e)
            {
                log.error(e);
                answer = HttpErrorHelper.toHttpResponse(e);
                return answer;
            }
            finally
            {
                HttpHeader header = _securityManager.getHeaderForResponse(authorization, answer.Status, answer.Entity);
                answer.putHeader(header.getName(), header.getValue());
            }
        }
    }
}
