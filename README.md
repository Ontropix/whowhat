# WhoWhat
[![Build Status](https://travis-ci.org/Ontropix/whowhat.svg?branch=master)](https://travis-ci.org/Ontropix/whowhat) <br />
Question-answer app for Windows Phone

## Description
The initial idea was to create a something similar to Stackoverflow.com, but picture is required. It would be usefull in the situation where you don't know where anything is located or how to name a specific thing. There is a source code for both client and server. 

## Client
- Windows Phone 8
- Telerik Windows Phone Controls
- Caliburn.Micro
- RestSharp to invoke the Server API
- Fody (for handy PropertyChanged generation)
- A set of custom controls. See /src/client/WhoWhat.UI.WindowsPhone/Controls

## Server 
- Our own [CQRS](https://en.wikipedia.org/wiki/Command%E2%80%93query_separation#Command_Query_Responsibility_Segregation) + Eventsorcing library /src/server/dependencies/platform. We're going to make it opensource soon.
- MongoDB as Event storage and Read database
- ServiceStack for REST API. Version 3.9.71, before it became commerical
- ServiceStack.Api.Swagger for API documentation and visualization
- StructureMap as IoC container
- PushSharp for push notification

The server has been hosted in Azure - 2 server instances, 1 MongoDB virtual machine, 1 Azure Redis Cache. 
Redis was used as session storage as well as contained users' score

