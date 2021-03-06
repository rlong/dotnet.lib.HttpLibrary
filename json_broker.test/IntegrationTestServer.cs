﻿// Copyright (c) 2013 Richard Long & HexBeerium
//
// Released under the MIT license ( http://opensource.org/licenses/MIT )
//

using System;
using System.Collections.Generic;
using System.Text;
using dotnet.lib.Http.server.reqest_handler;
using dotnet.lib.Http.server;
using dotnet.lib.Http.authentication.server;
using dotnet.lib.Http.json_broker;
using dotnet.lib.Http.json_broker.server;
using dotnet.lib.CoreAnnex.log;
using dotnet.lib.Http.authentication;
using dotnet.lib.Http.json_broker.test;

namespace dotnet.lib.Http.json_broker.test
{
    public class IntegrationTestServer
    {

        private static Log log = Log.getLog(typeof(IntegrationTestServer));

        ///////////////////////////////////////////////////////////////////////
        // openServicesRequestHandler
        private ServicesRequestHandler _openServicesRequestHandler;

        //public ServicesRequestHandler OpenServicesRequestHandler
        //{
        //    get { return _openServicesRequestHandler; }
        //    set { _openServicesRequestHandler = value; }
        //}

        ///////////////////////////////////////////////////////////////////////
        // authServicesRequestHandler
        private ServicesRequestHandler _authServicesRequestHandler;

        //public ServicesRequestHandler AuthServicesRequestHandler
        //{
        //    get { return _authServicesRequestHandler; }
        //    set { _authServicesRequestHandler = value; }
        //}


        ///////////////////////////////////////////////////////////////////////
        // webServer
        private WebServer _webServer;

        //public WebServer WebServer
        //{
        //    get { return _webServer; }
        //    set { _webServer = value; }
        //}



        public void AddService( DescribedService service ) 
        {
            _openServicesRequestHandler.AddService(service);
            _authServicesRequestHandler.AddService(service);

        }

        public void Start()
        {
            if (null != _webServer)
            {
                log.warn("null != _webServer");
                return;
            }

            RootRequestHandler rootProcessor = new RootRequestHandler();
            // open ... 
            {
                _openServicesRequestHandler = new ServicesRequestHandler();
                OpenRequestHandler openRequestHandler = new OpenRequestHandler();
                openRequestHandler.AddRequestHandler(_openServicesRequestHandler);

                rootProcessor.AddRequestHandler(openRequestHandler);
            }

            // auth ...
            {
                _authServicesRequestHandler = new ServicesRequestHandler();


                ServerSecurityConfiguration securityConfiguration = SecurityConfiguration.TEST;
                HttpSecurityManager httpSecurityManager = new HttpSecurityManager(securityConfiguration);
                AuthRequestHandler authRequestHandler = new AuthRequestHandler(httpSecurityManager);

                authRequestHandler.AddRequestHandler(_authServicesRequestHandler);


                rootProcessor.AddRequestHandler(authRequestHandler);
            }


            _webServer = new WebServer(rootProcessor);
            _webServer.Start();

        }


        public void Stop()
        {
            log.enteredMethod();

            if (null != _webServer)
            {
                _webServer.Stop();
            }
        }

    }
}
