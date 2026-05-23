# Migration Guide

**Document Control**

| **Version** | **Date**    | **Comment**                          |
|-------------|-------------|--------------------------------------|
| 6.0         | 23-MAY-2026 | Updated to cover version 6.0         |

- [Migration Guide](#migration-guide)
  - [Migrating from 4.3 to 5.0](#migrating-from-43-to-50)
  - [Migrating from 5.0 to 6.0](#migrating-from-50-to-60)

## Migrating from 4.3 to 5.0

### General Upgrades

#### Support .NET versions

Please be aware that only the following versions are supported:

- .NET 4.7.2, .NET 4.8, .NET 8.0, .NET 9.0, .NET 10.0

Please check the targets.props file for changes required for the sammple projects.

### Upgrade client application

#### Changes in the project (csproj) file

The affected part of the csproj file will then be:

```
  <ItemGroup>
    <PackageReference Include="Technosoftware.UaSolutions.UaUtilities" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaTypes" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaCore" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaConfiguration" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaClient" Version="5.0.0" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

#### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleClient.Config.xml.

#### Breaking changes in some methods

### Upgrade server application

#### Changes in the project (csproj) file

The affected part of the csproj file will then be:

```
  <ItemGroup Condition=" '$(NoHttps)' != 'true' ">
    <PackageReference Include="Technosoftware.UaSolutions.UaBindings.Https" Version="5.0.0" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Technosoftware.UaSolutions.UaUtilities" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaTypes" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaCore" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaConfiguration" Version="5.0.0" />
    <PackageReference Include="Technosoftware.UaSolutions.UaServer" Version="5.0.0" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

#### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleServer.Config.xml.

## Migrating from 5.0 to 6.0

### General Upgrades

#### Support .NET versions

Please be aware that only the following versions are supported:

- .NET 10.0

Please check the targets.props file for changes required for the sammple projects.

Version 6 uses a Directory.Packages.props file to manage package versions.

The important part of the props file:

```
  <ItemGroup>
  <ItemGroup>
    <PackageVersion Include="OPCFoundation.NetStandard.Opc.Ua.Types" Version="1.5.378.134" />
    <PackageVersion Include="OPCFoundation.NetStandard.Opc.Ua.Security.Certificates" Version="1.5.378.134" />
    <PackageVersion Include="OPCFoundation.NetStandard.Opc.Ua.Core" Version="1.5.378.134" />
    <PackageVersion Include="OPCFoundation.NetStandard.Opc.Ua.Bindings.Https" Version="1.5.378.134" />
```

For the UaConfiguration, UaClient and UaServer the source code is included in the solution and not referenced as a package.

### Upgrade client application

#### Changes in the project (csproj) file

The affected part of the csproj file will then be:

```
  <ItemGroup>
    <PackageReference Include="OPCFoundation.NetStandard.Opc.Ua.Core" />
  </ItemGroup>     
  <ItemGroup>
    <ProjectReference Include="..\..\..\src\Technosoftware\UaConfiguration\Technosoftware.UaConfiguration.csproj" />
    <ProjectReference Include="..\..\..\src\Technosoftware\UaClient\Technosoftware.UaClient.csproj" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

#### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleClient.Config.xml.

#### Breaking changes in some methods

### Upgrade server application

#### Changes in the project (csproj) file

The affected part of the csproj file will then be:

```
  <ItemGroup Condition=" '$(NoHttps)' != 'true' ">
    <PackageReference Include="OPCFoundation.NetStandard.Opc.Ua.Bindings.Https" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="OPCFoundation.NetStandard.Opc.Ua.Core" />
  </ItemGroup>   
  <ItemGroup>
    <ProjectReference Include="..\..\..\src\Technosoftware\UaConfiguration\Technosoftware.UaConfiguration.csproj" />
    <ProjectReference Include="..\..\..\src\Technosoftware\UaServer\Technosoftware.UaServer.csproj" />
    <ProjectReference Include="..\SampleNodeManagers\SampleCompany.SampleNodeManagers.csproj" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

#### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleServer.Config.xml.
