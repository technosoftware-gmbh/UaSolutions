# Distribution

## NuGet packages

The OPC UA Solutions .NET is delivered as a set of [NuGet packages](https://github.com/orgs/technosoftware-gmbh/packages). Depending on the targeted .NET Framework you have the following packages available:

### .NET 9.0, .NET 8.0, .NET 4.8 or .NET 4.7.2

- [Technosoftware.UaSolutions.UaClient](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaClient)

  The OPC UA Client .NET offers a fast and easy access to the OPC UA Client technology. Develop OPC UA compliant UA Clients with C#.

- [Technosoftware.UaSolutions.UaServer](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaServer)

  The OPC UA Server .NET offers a fast and easy access to the OPC Unified Architecture (UA) technology. Develop OPC UA&nbsp;compliant Servers with C#.

- [Technosoftware.UaSolutions.UaPubSub](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaPubSub)

  The OPC UA PubSub .NET offers a fast and easy access to the OPC Unified Architecture (UA) technology. Develop OPC UA compliant Publisher and Subscriber with C#.

- [Technosoftware.UaSolutions.UaConfiguration](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaConfiguration)

  The OPC UA Client and Server .NET uses a common set of configuration options which is the content of this package.

- [Technosoftware.UaSolutions.UaBindings.Https](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaBindings.Https)

  If your OPC UA Client or OPC UA Server needs support of HTTPS then you need to reference also this package.

- [Technosoftware.UaSolutions.UaCore](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaCore)

  Based on the OPC UA .NET Stack from the OPC Foundation. 

- [Technosoftware.UaSolutions.UaUtilities](https://github.com/technosoftware-gmbh/UaSolutions/pkgs/nuget/Technosoftware.UaSolutions.UaUtilities)

  License handling.

## Features Included
  
.NET 9.0 or .NET 8.0 allows you to develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10
(including embedded/IoT editions) without requiring platform-specific modifications.

Furthermore, cloud applications and services (such as ASP.NET, DNX, Azure Websites, Azure Webjobs, Azure Nano Server and Azure Service Fabric) are also supported.

### OPC UA Solution Core

- Fully ported Core OPC UA Stack.
- X.509 Certificate support for client and server authentication.
- SHA-2 support (up to SHA512) including security profile Basic256Sha256, Aes128Sha256RsaOaep and  Aes256Sha256RsaPss for configurations with high security needs.
- ECC Security policies ECC_nistP256, ECC_nistP384, ECC_brainpoolP256r1 and ECC_brainpoolP384r1.
- Anonymous, username and X.509 certificate user authentication.
- UA-TCP & HTTPS transports (client and server).
- Reverse Connect for the UA-TCP transport (client and server).
- Folder & OS-level (X509Store) Certificate Stores *Global Discovery Server* and *Server Push* support.
- Sessions and (durable) Subscriptions.
- Support for WellKnownRoles and RoleBasedUserManagement.