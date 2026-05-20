# Distribution

## NuGet packages for version 4 and 5

The OPC UA Solutions .NET is delivered as a set of [NuGet packages](https://github.com/orgs/technosoftware-gmbh/packages). Depending on the targeted .NET Framework you have the following packages available:

### .NET 10.0, .NET 9.0, .NET 8.0, .NET 4.8 or .NET 4.7.2

- [Technosoftware.UaSolutions.UaClient](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaClient)

  The OPC UA Client .NET offers a fast and easy access to the OPC UA Client technology. Develop OPC UA compliant UA Clients with C#.

- [Technosoftware.UaSolutions.UaServer](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaServer)

  The OPC UA Server .NET offers a fast and easy access to the OPC Unified Architecture (UA) technology. Develop OPC UA&nbsp;compliant Servers with C#.

- [Technosoftware.UaSolutions.UaConfiguration](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaConfiguration)

  The OPC UA Client and Server .NET uses a common set of configuration options which is the content of this package.

- [Technosoftware.UaSolutions.UaBindings.Https](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaBindings.Https)

  If your OPC UA Client or OPC UA Server needs support of HTTPS then you need to reference also this package.

- [Technosoftware.UaSolutions.UaCore](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaCore)

  Based on the OPC UA .NET Stack from the OPC Foundation. 

- [Technosoftware.UaSolutions.UaUtilities](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaUtilities)

  License handling.


## NuGet packages for version 6

The OPC UA Solutions .NET is delivered as source but uses some NuGet packages:

### .NET 10.0

- [OPCFoundation.NetStandard.Opc.Ua.Bindings.Https](https://www.nuget.org/packages/OPCFoundation.NetStandard.Opc.Ua.Bindings.Https)

  The Https binding is an optional component to support UA over Https for 'opc.https' endpoints.

- [OPCFoundation.NetStandard.Opc.Ua.Types](https://www.nuget.org/packages/OPCFoundation.NetStandard.Opc.Ua.Types)

  Types is required for Client and Server projects.

- [OPCFoundation.NetStandard.Opc.Ua.Core](https://www.nuget.org/packages/OPCFoundation.NetStandard.Opc.Ua.Core)

  Core is required for Client and Server projects.

- [OPCFoundation.NetStandard.Opc.Ua.Security.Certificates](https://www.nuget.org/packages/OPCFoundation.NetStandard.Opc.Ua.Security.Certificates/)

  Certificates is required for Client and Server projects.


## Features Included
  
.NET 10.0, .NET 9.0 or .NET 8.0 allows you to develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10
(including embedded/IoT editions) without requiring platform-specific modifications.

Furthermore, cloud applications and services (such as ASP.NET, DNX, Azure Websites, Azure Webjobs, Azure Nano Server and Azure Service Fabric) are also supported.

### Key Features and Updates in OPC UA 1.05

* **Thread Safety and Locking**: Improved thread safety and reduced locking in secure channel operations.
* **Audit and Redaction**: New interfaces for auditing and redacting sensitive information.
* **Security Enhancements**: Improved encryption and authentication mechanisms.
* **CRL Support**: Added Certificate Revocation List support for X509Store on Windows.
* **Performance Improvements**: Faster binary encoding and decoding, reducing memory usage and latency.
* **Role-Based Management**: Support for WellKnownRoles and RoleBasedUserManagement.
* **Improved Logging**: Enhanced logging with `ILogger` and `EventSource`.
* **ECC Profiles**: Support for NIST & Brainpool.
* **Durable Subscriptions**: Support for Durable Subscriptions.