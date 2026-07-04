# Sample Applications

The OPC UA Client .NET contains sample client applications, you can find them in 

- /tutorials/SampleCompany

## Required NuGet packages

The OPC UA CLient .NET is divided into several DLL’s as shown in the picture below:

![](../images/OPCUANETArchitecture.png)

The DLLs are delivered as local NuGet Packages. The OPC UA Client .NET uses the following packages:


| **Name**                                                   | **Description**                                                                                    |
|:-----------------------------------------------------------|:---------------------------------------------------------------------------------------------------|
| **Technosoftware.UaSolutions.UaUtilities**                 | The Utilities Class Library (**Version 4 and 5 only**).                                            |
| **Technosoftware.UaSolutions.UaTypes**                     | The OPC UA Type Library (**Version 5 only**).                                                      |
| **Technosoftware.UaSolutions.UaCore**                      | The OPC UA Core Class Library (**Version 4 and 5 only**).                                          |
| **OPCFoundation.NetStandard.Opc.Ua.Types**                 | The OPC UA Type Library (**Version 6 only**).                                                      |
| **OPCFoundation.NetStandard.Opc.Ua.Security.Certificates** | The OPC UA Security X509 Certificates Class Library (**Version 6 only**).                          |
| **OPCFoundation.NetStandard.Opc.Ua.Core**                  | The OPC UA Core Class Library (**Version 6 only**).                                                |
| **Technosoftware.UaSolutions.UaConfiguration**             | Contains configuration related classes like, e.g. ApplicationInstance.                             |
| **Technosoftware.UaSolutions.UaClient**                    | The OPC UA Client Class library containing the classes and methods usable for client development.  |

## Solution

The main OPC UA Solution can be found in the root of the repository and is named.

- Tutorials.slnx

The solution contains two sample clients, as well as two sample servers used by these clients.

## Prerequisites

Once the dotnet command is available, navigate to the root folder in your local copy of the repository / and execute the following command:

dotnet restore /p:Configuration=Debug /p:Platform="Any CPU" Tutorials.slnx

This command restores the tree of dependencies.

## Start the server

1.  Open a command prompt.
2.  Navigate to the folder tutorials/SampleCompany/Simple/SampleServer.
3.  To run the server sample type  
       
    dotnet run --no-restore --framework net8.0 --project SampleCompany.SampleServer.csproj --autoaccept
    -   The server is now running and waiting for connections.
    -   The --autoaccept flag allows to auto accept unknown certificates and should only be used to simplify testing.

## Start the client

1.  Open a command prompt.
2.  Navigate to the folder tutorials/SampleCompany/Simple/SampleClient.
3.  To run the client sample type   
      
    dotnet run --no-restore --framework net8.0 --project SampleCompany.SampleClient.csproj --autoaccept
    -   The client connects to the OPC UA console sample server running on the same host.
    -   The --autoaccept flag allows to auto accept unknown certificates and should only be used to simplify testing.
4.  If not using the --autoaccept auto accept option, on first connection, or after certificates were renewed, the server may have refused the client certificate. Check the server and client folder %LocalApplicationData%/OPC Foundation/pki/rejected for rejected certificates. To approve a certificate copy it to the %LocalApplicationData%/OPC Foundation/pki/trusted.

## Check the output

If everything was done correctly the client should show the following lines:

```
OPC UA Simple Console Sample Client
WARNING: No valid license applied.
Connecting to... opc.tcp://localhost:62555/SampleServer
New Session Created with SessionName = SampleCompany OPC UA Sample Client
Connected! Ctrl-C to quit.
Reading server status...
   Read Value = {11.02.2024 10:27:14 | 11.02.2024 10:27:26 | Running | Opc.Ua.BuildInfo | 0 | } , StatusCode = Good
   Read Value = StartTime , StatusCode = Good
   Read Value = 11.02.2024 10:27:14 , StatusCode = Good
Reading nodes...
Read Value = {11.02.2024 10:27:14 | 11.02.2024 10:27:26 | Running | Opc.Ua.BuildInfo | 0 | } , StatusCode = Good
Read Value = StartTime , StatusCode = Good
Read Value = 11.02.2024 10:27:14 , StatusCode = Good
Reading Value of NamespaceArray node...
NamespaceArray Value = {http://opcfoundation.org/UA/|urn:technosoftware:SampleCompany:SampleServer|http://samplecompany.com/SampleServer/Model|http://opcfoundation.org/UA/Diagnostics}
Writing nodes...
Write Results :
     Good
     Good
     Good
Browsing i=2253 node...
Browse returned 19 results:
```