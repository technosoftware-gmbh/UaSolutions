# Upgrade from 3.4 to 4.2

[TOC]

## General Upgrades

### Support .NET versions

Please be aware that only the following versions are supported:

- .NET 4.7.2, .NET 4.8, .NET 8.0, .NET 9.0

Please check the targets.props file for changes required for the sammple projects.

## Upgrade client application

### Changes in the project (csproj) file

The following package must be added:

- Technosoftware.UaSolutions.UaUtilities

The following changes in assemblies must be done:

- Technosoftware.UaSolution.UaBindings.Https -> Technosoftware.UaSolutions.UaBindings.Https
- Technosoftware.UaSolution.UaCore -> Technosoftware.UaSolutions.UaCore
- Technosoftware.UaSolution.UaConfiguration -> Technosoftware.UaSolutions.UaConfiguration
- Technosoftware.UaSolution.UaClient -> Technosoftware.UaSolutions.UaClient

The affected part of the csproj file will then be:

```
  <ItemGroup>
    <PackageReference Include="Technosoftware.UaSolutions.UaUtilities" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaCore" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaConfiguration" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaClient" Version="4.2.0" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleClient.Config.xml.

### Breaking changes in some methods

- DeleteApplicationInstanceCertificateAsync has an additional parameter or can be used without one
- CheckApplicationInstanceCertificateAsync was replaced with CheckApplicationInstanceCertificateAsync


## Upgrade server application

### Changes in the project (csproj) file

The following package must be added:

- Technosoftware.UaSolutions.UaUtilities

The following changes in assemblies must be done:

- Technosoftware.UaSolution.UaBindings.Https -> Technosoftware.UaSolutions.UaBindings.Https
- Technosoftware.UaSolution.UaCore -> Technosoftware.UaSolutions.UaCore
- Technosoftware.UaSolution.UaConfiguration -> Technosoftware.UaSolutions.UaConfiguration
- Technosoftware.UaSolution.UaServer -> Technosoftware.UaSolutions.UaServer

If you use the server development using the UaBaseServer you need to change one of the following:

- Technosoftware.UaSolution.UaBaseServer -> Technosoftware.UaSolutions.UaBaseServer

The affected part of the csproj file will then be:

```
  <ItemGroup Condition=" '$(NoHttps)' != 'true' ">
    <PackageReference Include="Technosoftware.UaSolutions.UaBindings.Https" Version="4.2.0" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Technosoftware.UaSolutions.UaUtilities" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaCore" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaConfiguration" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaServer" Version="4.2.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaBaseServer" Version="4.2.0" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleServer.Config.xml.




