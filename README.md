# WhoWhat
[![Build Status](https://travis-ci.org/Ontropix/whowhat.svg?branch=master)](https://travis-ci.org/Ontropix/whowhat)
Question-answer app for Windows Phone

# Description
The initial idea was to create a something similar to Stackoverflow.com, but picture is required. It would be usefull in the situation where you don't know where anything is located or how to name a specific thing. There is a source code for both client and server. 

# Client
- Windows Phone 8
- Telerik Windows Phone Controls
- Caliburn.Micro
- RestSharp
- Fody (for handy PropertyChanged generation)
- A bunch of custom controls. See /src/client/WhoWhat.UI.WindowsPhone/Controls

# Server 
- Our own CQRS+Eventsorcing library src/server/dependencies/platform. We're going to make it opensource soon.
- MongoDB event storage
- ServiceStack. Version 3.9.71, before it became commerical
- ServiceStack.Api.Swagger for API documentation and visualization
- StructureMap as IoC container
- PushSharp for push notification

The server has been hosted in Azure - 2 server instances, 1 MongoDB virtual machine, 1 Redis. 
Redis was used as session storage as well as contained current users' score

