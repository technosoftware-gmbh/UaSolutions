# Technosoftware GmbH OPC UA CLient Gateway .NET

## Overview

The OPC UA Client Gateway .NET  allows any OPC UA Clients to access Classic OPC DA, AE and HDA servers. 

The OPC UA Client Gateway .NET can be configured with an integrated configuration tool. With this tool you can specify the Classic OPC Server to be used as well as starting node (folder) within the UA address space to be used for mapping the Classic OPC Server address space.

## Configuration

The configuration is done in the Config.xml file as an extension, e.g.:

```
  <Extensions>
    <ua:XmlElement>
      <ComWrapperServerConfiguration xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://opcfoundation.org/UA/SDK/COMInterop">
        <WrappedServers>
          <ComClientConfiguration i:type="ComDaClientConfiguration">
            <ServerUrl>opc.com://localhost/Technosoftware.DaSample</ServerUrl>
            <ServerName>DA</ServerName>
            <MaxReconnectWait>100000</MaxReconnectWait>
            <SeperatorChars></SeperatorChars>
            <AvailableLocales xmlns:d4p1="http://opcfoundation.org/UA/2008/02/Types.xsd" i:nil="true" />
            <BrowseToSupported>false</BrowseToSupported>
          </ComClientConfiguration>
          <ComClientConfiguration i:type="ComAeClientConfiguration">
            <ServerUrl>opc.com://localhost/Technosoftware.AeSample</ServerUrl>
            <ServerName>AE</ServerName>
            <MaxReconnectWait>100000</MaxReconnectWait>
            <SeperatorChars></SeperatorChars>
            <AvailableLocales xmlns:d4p1="http://opcfoundation.org/UA/2008/02/Types.xsd" i:nil="true" />
          </ComClientConfiguration>
        </WrappedServers>
        <ClassicWrapperAddress i:nil="true" />
      </ComWrapperServerConfiguration>
    </ua:XmlElement>
  </Extensions>
```

