# Upgrade from 4.2 to 5.0

## General Upgrades

### Support .NET versions

Please be aware that only the following versions are supported:

- .NET 4.7.2, .NET 4.8, .NET 8.0, .NET 9.0, .NET 10.0

Please check the targets.props file for changes required for the sammple projects.

## Upgrade client application

### Changes in the project (csproj) file

The affected part of the csproj file will then be:

```
  <ItemGroup>
    <PackageReference Include="Technosoftware.UaSolutions.UaUtilities" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaCore" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaConfiguration" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaClient" Version="5.0.0-rc" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleClient.Config.xml.

### Breaking changes in some methods

## Upgrade server application

### Changes in the project (csproj) file

The affected part of the csproj file will then be:

```
  <ItemGroup Condition=" '$(NoHttps)' != 'true' ">
    <PackageReference Include="Technosoftware.UaSolutions.UaBindings.Https" Version="5.0.0-rc" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Technosoftware.UaSolutions.UaUtilities" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaCore" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaConfiguration" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaServer" Version="5.0.0-rc" />
    <PackageReference Include="Technosoftware.UaSolutions.UaBaseServer" Version="5.0.0-rc" />
  </ItemGroup>
```

There might be other updates needed to package versions from Microsoft or other parties depending on what is used. You can check the SampleCompany.SampleClient.csproj for the changes required for the sample client.

### Changes in the configuration file

Depending on the version you are coming from you should compare the changes made in the configuration file. For the sample client application you can find it in SampleCompany.SampleServer.Config.xml.




