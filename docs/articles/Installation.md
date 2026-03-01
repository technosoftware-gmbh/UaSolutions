# Installation and Administration

## Installation .NET

Please follow instructions in this [article](https://dotnet.microsoft.com/en-us/learn/dotnet/hello-world-tutorial/intro) to setup the dotnet command line environment for your platform.

As of today, .NET 9.0, or .NET 8.0 is required. The article describes the installation of .NET 8.0.101 for Windows, Linux and macOS. This version also works with the OPC UA Client and Server Solutions we provide in this GitHub repositories.

Please follow at least the sections:

* Intro
* Download and Install

to install the .NET SDK. You find the different .NET versions also [here](https://dotnet.microsoft.com/en-us/download/dotnet).

## Environments used for testing

This GitHub repository is automatically built with the following environments:

- Linux Ubuntu 24.04.2 LTS
  - .NET 9.0.x
- Mac OS X 14.7.4
  - .NET 9.0.x
- Microsoft Windows Server 2022 10.0.20348
  - .NET 9.0.x

##  Directory Structure
The repository contains the following basic directory layout:

- documentation/
  - Installation.md

    Installation and Administration of .NET based OPC UA Applications
  
    Additional documentation:
    - OPC_UA_Solution_NET_Introduction.pdf

      Introduction in Developing OPC UA Clients and OPC UA Servers with C# / VB.NET

- examples/
  
  Sample server applications based on the UaBaseServer library using the nuget packages

- licenses/
  
  Licenses applying

- nuget/

  nuget specification files used to build the nuget packages
  - packages
  
    Local nuget package repository

- tutorials/

  Sample client and server applications using the nuget packages. The sample server are based on the UaStandardServer library and can use more than one nodemanager in one server.

- Workshop/

  OPC UA Workshop content as PDF

##  DLLs used by applications

The solution consists of the following main components:

- Technosoftware.UaSolutions.UaCore.dll
- Technosoftware.UaSolutions.UaBindings.Https.dll
- Technosoftware.UaSolutions.UaConfiguration.dll
- Technosoftware.UaSolutions.UaUtilities.dll
  
These DLLs are used by all applications using the solution. In addition, one or several of the following DLL’s might be required:

- Technosoftware.UaSolutions.UaClient.dll
  
  Client Applications require this DLL.
- Technosoftware.UaSolutions.UaServer.dll
  
  Server Applications require this DLL.
- Technosoftware.UaSolutions.UaPubSub.dll
  
  PubSub Applications require this DLL.
  
Depending on which features server applications uses you also need to use one of the following DLLs:

- Technosoftware.UaSolutions.UaBaseServer.dll
  
  Server Applications using the original features from V2.x require this DLL. See **[here](tutorials/SampleCompany/Simple)**.
- Technosoftware.UaSolutions.UaStandardServer.dll
  
  Advanced server Applications with more than one node manager require this DLL. See **[here](tutorials/SampleCompany/Advanced)**.

These DLLs are delivered via NuGet Packages available [here](/nuget/packages):

- Technosoftware.UaSolutions.UaCore
- Technosoftware.UaSolutions.UaBindings.Https
- Technosoftware.UaSolutions.UaUtilities
- Technosoftware.UaSolutions.UaConfiguration
- Technosoftware.UaSolutions.UaClient
- Technosoftware.UaSolutions.UaServer
- Technosoftware.UaSolutions.UaPubSub
- Technosoftware.UaSolutions.UaBaseServer
- Technosoftware.UaSolutions.UaStandardServer

## OPC UA Local Discovery Server
  
The Local Discovery Server (LDS) is a DiscoveryServer that maintains a list of all UA Servers and Gateways available on the host/PC that it runs on and is the UA equivalent to the OPC Classic OPCENUM interface.
An LDS is a service that runs in the background. UA Servers will periodically connect to the LDS and Register themselves as being available. This periodic activity means that the list of available UA servers is always current and means that a client can immediately connect to any of them (security permissions pending).
The OPC UA Local Discovery Server is an installation from the OPC Foundation and delivered as installation executable and as merge module. You can download it [here](https://opcfoundation.org/developer-tools/samples-and-tools-unified-architecture/local-discovery-server-lds/).

## Test your installation with .NET 9.0
The main OPC UA Solution can be found in the root of the repository and is named.

- Tutorials.sln

The solution contains a sample client, as well as a sample server used by this client for both the simple and advanced examples.

###  Prerequisites

Once the `dotnet` command is available, navigate to the root folder in your local copy of the repository and execute `dotnet restore /p:Configuration=Debug /p:Platform="Any CPU" Tutorials.sln`.

This command restores the tree of dependencies.

### Start the server

1. Open a command prompt.
2. Navigate to the folder **tutorials/SampleCompany/Advanced/SampleServer**.
3. To run the server sample type

   `dotnet run --no-restore --framework net9.0 --project SampleCompany.SampleServer.csproj --autoaccept`

   - The server is now running and waiting for connections.
   - The `--autoaccept` flag allows to auto accept unknown certificates and should only be used to simplify testing.

### Start the client
1. Open a command prompt.
2. Navigate to the folder **tutorials/SampleCompany/Advanced/SampleClient**.
3. To run the client sample type
   `dotnet run --no-restore --framework net9.0 --project SampleCompany.SampleClient.csproj --autoaccept`
   
   - The client connects to the OPC UA console sample server running on the same host.
   - The `--autoaccept` flag allows to auto accept unknown certificates and should only be used to simplify testing.

4. If not using the `--autoaccept` auto accept option, on first connection, or after certificates were renewed, the server may have refused the client certificate. Check the server and client folder %LocalApplicationData%/OPC Foundation/pki/rejected for rejected certificates. To approve a certificate copy it to the %LocalApplicationData%/OPC Foundation/pki/trusted.

### Check the output
If everything was done correctly the client should show the following lines:

```
OPC UA Advanced Console Sample Client
WARNING: No valid license applied.
Connecting to... opc.tcp://localhost:62555/SampleServer
New Session Created with SessionName = SampleCompany OPC UA Sample Client
Connected! Ctrl-C to quit.
Reading nodes...
Read Value = {11.02.2024 10:28:49 | 11.02.2024 10:29:18 | Running | Opc.Ua.BuildInfo | 0 | } , StatusCode = Good
Read Value = StartTime , StatusCode = Good
Read Value = 11.02.2024 10:28:49 , StatusCode = Good
Reading Value of NamespaceArray node...
NamespaceArray Value = {http://opcfoundation.org/UA/|urn:technosoftware:SampleCompany:SampleServer|http://samplecompany.com/SampleServer/NodeManagers/Simulation|http://opcfoundation.org/UA/Diagnostics}
Writing nodes...
Write Results :
     Good
     Good
     Good
Browsing i=2253 node...
Browse returned 19 results:
     DisplayName = ServerArray, NodeClass = Variable
     DisplayName = NamespaceArray, NodeClass = Variable
     DisplayName = UrisVersion, NodeClass = Variable
     DisplayName = ServerStatus, NodeClass = Variable
     DisplayName = ServiceLevel, NodeClass = Variable
     DisplayName = EstimatedReturnTime, NodeClass = Variable
     DisplayName = LocalTime, NodeClass = Variable
     DisplayName = ServerCapabilities, NodeClass = Object
     DisplayName = ServerDiagnostics, NodeClass = Object
     DisplayName = VendorServerInfo, NodeClass = Object
     DisplayName = ServerRedundancy, NodeClass = Object
     DisplayName = Namespaces, NodeClass = Object
     DisplayName = ServerConfiguration, NodeClass = Object
     DisplayName = Quantities, NodeClass = Object
     DisplayName = DefaultHAConfiguration, NodeClass = Object
     DisplayName = DefaultHEConfiguration, NodeClass = Object
     DisplayName = PublishSubscribe, NodeClass = Object
     DisplayName = Dictionaries, NodeClass = Object
     DisplayName = Resources, NodeClass = Object
Calling UA method for node ns=2;s=Methods_Hello ...
Method call returned 1 output argument(s):
     OutputValue = hello from Call Method
New Subscription created with SubscriptionId = 1072294259.
MonitoredItems created for SubscriptionId = 1072294259.
Waiting...
Notification: 1 "ns=2;s=Scalar_Simulation_Int32" and Value = 38825327.
Notification: 1 "ns=2;s=Scalar_Simulation_Int32" and Value = 184268556.
Notification: 1 "ns=2;s=Scalar_Simulation_Float" and Value = 1.0285675E+33.
Notification: 1 "ns=2;s=Scalar_Simulation_Float" and Value = -1.3104288E+32.
Notification: 1 "ns=2;s=Scalar_Simulation_String" and Value = ??????= ??????] ??????] ????? ????_ ??????????.
Notification: 1 "ns=2;s=Scalar_Simulation_String" and Value = ?????????& ??????????. ???- ????????- ?????? ?????? ???????< ????????. ?????? ?????? ?????.
Notification: 2 "ns=2;s=Scalar_Simulation_Int32" and Value = 332084715.
Notification: 2 "ns=2;s=Scalar_Simulation_Float" and Value = -3.7500198E-34.
Notification: 2 "ns=2;s=Scalar_Simulation_String" and Value = ?????? ???????? ????? ????? ??????$ ????' ?????? ???????????.
Notification: 3 "ns=2;s=Scalar_Simulation_Int32" and Value = 103429018.
Notification: 3 "ns=2;s=Scalar_Simulation_Float" and Value = -1.3437527E-14.
Notification: 3 "ns=2;s=Scalar_Simulation_String" and Value = Elefante Elefante` Arándano Gato Rata[ Oveja" Fresa Negro Dragón( Elefante.
```
