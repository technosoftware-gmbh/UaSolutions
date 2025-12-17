# Changelog

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 5.0.0 Release Candidate

### New features:

#### Server

- Support for async method calls by implementing IAsyncNodeManager interface
- New Task based Request Queue
- New generated Code with async Method Call handlers & async Service Calls
- New extended CustomNodeManager with support for SamplingGroup Mechanism

### Changes:

- Enhanced license handling for support contracts

### Breaking Changes:

#### Server

- UaServer: All classes are now under one namespacer (Technosoftware.UaServer)
- IUaMonitoredItem Interface extended & used instead of UaMonitoredItem Class
- New IUaSession Interface instead of Session Class
- IUaSesssionManager, IUaSubscriptionManager, IUaSubscription & IuaServerData interfaces extended
- Renamed UaImpersonateUserEventArgs to ImpersonateUserEventArgs
- Renamed several event handlers to correct naming issues, e.g. SessionCreatedEvent -> SessionCreated
- Merged UaStandardServer into UaServer, simplifying class usages. UaGenericServer and UaGenericNodeManager no longer exists

#### Client

- All sync methods are now marked as deprecated
- DataTypeDictionaries moved into the ComplexTypes Package. If the helper methods of the Session were used we recommend to use the complex type system as a replacement that can handle all servers from OPC UA Spec 1.0 - 1.0.05.
- NodeCache now has an async interface (to have access to the ITypeTable interface use the .TypeTree property or AsNodeTable method 
- ApplicationInstance Methods Returning a task have Async suffixes

### Fixes:

#### Server

- Fix Subscription Keepalive being sent on first publish
- Handle unsent requests on closed SecureChannel to be sent on new channel of the same session

#### Client

- Disable Subscription cleanup if subscriptions are being created
- Fix subscription leaking due to not completed PublishResponseMessageWorkerAsync tasks
- Fix subscription leaking due to not completed PublishResponseMessageWorkerAsync tasks

### Integrated OPC UA Stack Versions

The following OPC UA Foundation releases of the OPC UA Stack were integrated:

- 1.5.378.10-preview
- 1.5.377.21

Detailed release notes are available [here](https://github.com/OPCFoundation/UA-.NETStandard/releases). Some of the changes are listed below.

#### Breaking Changes:

- ICertificateStore Async Methods now have async suffixes, old names deprecated
- All obsolete Methods removed

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.2.0

### Breaking Changes:
- Merged UaStandardServer into UaServer, simplifying class usages. UaGenericServer and UaGenericNodeManager no longer exists
- Renamed IUaNodeManager to IUaStandardNodeManager and IUaBaseNodeManager to IUaNodeManager

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.1.1

### Security Fix
- Fixed Microsoft Security Advisory [CVE-2025-55315](https://github.com/advisories/GHSA-5rrx-jjjq-q2r5) | NET Security Feature Bypass Vulnerability (Bump Microsoft.AspNetCore.Server.Kestrel.Core from 2.3.0 to 2.3.6)

### Changes:

- Updated dependencies

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.1.0

### Changes:

- Enhanced license handling for support contracts

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.0.4

### Changes:

- Enhanced license handling for upcoming product releases
- Updated dependencies

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.0.3

### Changes:

- Documentation related changes

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.0.2

This release is based on the 1.05.05 Nodeset with generated files from the ModelCompiler.
It contains important bug fixes which were found after the last OPC UA Core Stack 1.05.376 release,
as well as new features.

### Integrated OPC UA Stack Versions

The following OPC UA Foundation releases of the OPC UA Stack were integrated:

- 1.5.376.244
- 1.5.376.235
- 1.5.376.213
- 1.5.375.457
- 1.5.374.176
- 1.5.375.443
- 1.5.374.168
- 1.5.374.158
- 1.5.374.126
- 1.5.374.124
- 1.5.374.118

Detailed release notes are available [here](https://github.com/OPCFoundation/UA-.NETStandard/releases). Some of the changes are listed below.

### Enhancements

- Enhance enum definition handling and validation.
- Enhance role permission validation on server.

### New Features

- Implement Support for PEM Public Keys in Directory Certificate Store.
- Allow to modify sensitivity of KeepAliveStopped on Client.
- Add support for Async Service Calls in the generated files.
- Allow setting a temporary Context for using IEncoders in custom code
- Add Support for Durable Subscriptions by implementing needed persistence code in custom Interfaces.

### Breaking Changes:

- Updated XML-Encoding / XML-Decoding of Matrix Element in Variant to conform to Specification
- Interface extensions in the Server to support Durable Subscriptions
- A valid CertificateStore Configuration is now enforced on Startup

### Fixes

- Fix Client removing subscription if publish response is received before createSubscriptionResponse.
- Fix Subscription Diagnostics DataChangeNotificationsCount being calcualted incorrectly.
- Fix XML Encoding and decoding of Matrix element to conform to specification.
- Fix closing of shared Transport Channel in Recreate Scenario on client.
- Fix Regression - Sampling Group disposing m_shutdownEvent in Shutdown Method on server.
- Fix ValueRank for Base Variable State
- UserIdentityToken now uses UTF-8 Encoded byte Array for storing unencrypted password.
- Fix: SecurityToken renewal
- Remove oldest channel if not used by session
- Fix reconnect when ReverseConnection is used
- Add support for SupportsFilteredRetain
- Fix RegisterWithDiscoveryServer method not using a certificate for the secure channel

### NET target added

- The NET 9.0 target has been added.

### NET target removed

- The NET 7.0 outdated target has been removed.
- The NET 6.0 outdated target has been removed.

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.0.1

### Enhancements
- Update to 1.05.04 nodeset

### Fixes
- Fixed handling of fields if they are not in the exact same order (CompareDistinguishedNameFields)

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 4.0.0

### New Features
- Support of .NET 9.0

### Enhancements
- Security Enhancements

### Breaking Changes
- Removed support of .NET 7.0. End of Support was 14-MAY-2024.
- Splitted UaCore and added Technosoftware.UaCore.Security.Certificates and Technosoftware.UaUtilities

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.4.1

### Changes:

- Documentation related changes

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.4.0

This release is based on the 1.05.05 Nodeset with generated files from the ModelCompiler.
It contains important bug fixes which were found after the last OPC UA Core Stack 1.05.376 release,
as well as new features.

### Integrated OPC UA Stack Versions

The following OPC UA Foundation releases of the OPC UA Stack were integrated:

- 1.5.376.244
- 1.5.376.235
- 1.5.376.213
- 1.5.375.457
- 1.5.374.176
- 1.5.375.443
- 1.5.374.168
- 1.5.374.158
- 1.5.374.126
- 1.5.374.124
- 1.5.374.118

Detailed release notes are available [here](https://github.com/OPCFoundation/UA-.NETStandard/releases). Some of the changes are listed below.

### Enhancements

- Enhance enum definition handling and validation.
- Enhance role permission validation on server.

### New Features

- Implement Support for PEM Public Keys in Directory Certificate Store.
- Allow to modify sensitivity of KeepAliveStopped on Client.
- Add support for Async Service Calls in the generated files.
- Allow setting a temporary Context for using IEncoders in custom code
- Add Support for Durable Subscriptions by implementing needed persistence code in custom Interfaces.

### Breaking Changes:

- Updated XML-Encoding / XML-Decoding of Matrix Element in Variant to conform to Specification
- Interface extensions in the Server to support Durable Subscriptions
- A valid CertificateStore Configuration is now enforced on Startup

### Fixes

- Fix Client removing subscription if publish response is received before createSubscriptionResponse.
- Fix Subscription Diagnostics DataChangeNotificationsCount being calcualted incorrectly.
- Fix XML Encoding and decoding of Matrix element to conform to specification.
- Fix closing of shared Transport Channel in Recreate Scenario on client.
- Fix Regression - Sampling Group disposing m_shutdownEvent in Shutdown Method on server.
- Fix ValueRank for Base Variable State
- UserIdentityToken now uses UTF-8 Encoded byte Array for storing unencrypted password.
- Fix: SecurityToken renewal
- Remove oldest channel if not used by session
- Fix reconnect when ReverseConnection is used
- Add support for SupportsFilteredRetain
- Fix RegisterWithDiscoveryServer method not using a certificate for the secure channel

### NET target added

- The NET 9.0 target has been added.

### NET target removed

- The NET 7.0 outdated target has been removed.
- The NET 6.0 outdated target has been removed.

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.3.2

### Enhancements
- Improved error messages and error handling


### Known Issues
- Synchronous Reconnect unstable

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.3.1

### Security Fix
- Fixed Microsoft Security Advisory [CVE-2024-38095]( https://github.com/advisories/GHSA-447r-wph3-92pm) | .NET Denial of Service Vulnerability (Bump System.Formats.Asn1 from 8.0.0 to 8.0.1)

### Changes
- Bump BouncyCastle.Cryptography from 2.3.1 to 2.4.0
- Bump MQTTnet from 4.3.3.952 to 4.3.6.1152

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.3.0

### Enhancements
- Added Role based User management
- NuGet packages are now available as Github nuget registry

### Changes
- Updated to OPC UA 1.05.03
- Bump BouncyCastle.Cryptography from 2.3.0 to 2.3.1
- Improve client for sessions and subscriptions
- Moved reference applications to the **reference** directoy
- New tutorial examples can be found now in the **tutorials** directory
- Added GetApplicationInstanceCertificateExpiryDateAsync()
- The Technosoftware.UaCore nuget package now includes support again for .NET 4.7.2 and .NET 4.6.2 to be 
  able to create custom builds of the solutions
- Validates the ServerCertificate ApplicationUri to match the ApplicationUri of the Endpoint for an open call (Spec Part 4 5.4.1)
- Implement OpenSecureChannel in compliance with Spec Part 5.4.1
- check Application URI of the server Certificate on OpenSecureChannel
- The ApplicationUri specified in the Server Certificate must match the ApplicationUri provided in the EndpointDescription.

### Fixed issues
- Use EphemeralKeySet for certificates with private key on supporting platforms

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.2.3

### Changes
- NuGet packages are now only distributed locally in the repositories
- Added support of .NET 8.0
- Make CertStoreIdentifier.GetCertificateStoreTypeByName and RegisteredStoreTypeNames public

### Fixed issues
- Fix for references that may cause a memory leak
- GDS: add ArgumentException to Method CreateCACertificateAsync
- Array xmlns encoding/decoding always uses UA namespace
- Implement the close async codepath
- Implement the reconnect and republish with async service calls

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.2.2

### Changes
- Includes all changes and fixes from the [OPC UA 1.04 Maintenance Update 1.4.372.76](https://github.com/OPCFoundation/UA-.NETStandard/releases/tag/1.4.372.76).
- Integrated PubSub

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.9

### Fixed issues
- Fix for 'BadTooManyOperations' in session.publish / 'BadServerHalted' when server is stopped.
- Fixed status code when server is shutdown
- Add latest statuscodes from 1.05.03 to support new BadServerTooBusy error code
- Added default throttle delay in case an unknown error is returned on a publish request service call
- Removed extra unnecessary code and fixed use of userWriteMask
- Removed extra unnecessary code in  CompareArray<T>
- Fixed userWriteMask

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.8

### Changes
- Fixed Microsoft Security Advisory [CVE-2023-38180](https://github.com/OPCFoundation/UA-.NETStandard/releases/tag/1.4.371.91): .NET Denial of Service Vulnerability
- Support for session reconnect from distributed clients
- Truncate inner diagnostics if nested depth too high
- Better NodeId/ExpandedNodeId built in types handling
- Zip embedded Opc.Ua.NodeSet2.xml to reduce assembly size
- Improve ArraySegmentStream hotpath
- Always use ArrayPool.Shared for optimized use of buffers in buffer manager
- Fix corner cases of subscription transfer
- '#' is not treated as a reserved character in RelativePath's text format
- Fixed NodeId.CompareTo() null reference exception
- Refactor IEncoder/IDecoder to derive from IDisposable
- Support client activities on all target platforms

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.5

### Changes
- Support use of opc.https endpoint url for client and server.
- Support custom cert store with flat directory structure

### Fixed issues
- Fixed issues with use of opc.https introduced in 3.1.4.

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.4

### Changes
- Support use of opc.https endpoint url for client and server.

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.3

### Changes
- Removed unused System.Security.Cryptography.Pkcs

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.1

### Changes
- Includes all changes and fixes from the [OPC UA 1.04 Maintenance Update 1.4.371.91](https://github.com/OPCFoundation/UA-.NETStandard/releases/tag/1.4.371.91).

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.1.0

### Breaking Changes
- Removed support of .NET 4.6.2 and .NET 4.7.2
- SendCertificateChain default is now true

### Fixed issues
- Fixed issue in macOS, when cert chains are used the X509Certificate2 constructor throws exception
- Do not dispose the stream if the BinaryEncoder leaveopen flag is set in the constructor + ported tests from JSON
- Improve hashcode calculation for some built in types
- Fixes to support structures with allowsubtypes
- Close socket if a client stops processing responses.

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.0.5

### Changes
- Updated some dependencies

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.0.4

### Changes
- Includes all changes and fixes from the [OPC UA 1.04 Maintenance Update 1.4.371.60](https://github.com/OPCFoundation/UA-.NETStandard/releases/tag/1.4.371.60).

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.0.3

### Changes
- Added support of PubSub

-------------------------------------------------------------------------------------------------------------
## OPC UA Solutions .NET - 3.0.2

### Breaking Changes
- License
  - A new license ist needed. Old licenses from 2.3 or earlier will no longer work with 3.0 and above.
  - Product puchases from 2021 and 2022 can get a new license free of charge. Either through their online account or by sending us an Email.
  - All others can order an OPC UA Support subscription incl. Update [here](https://technosoftware.com/product/opc-support-subscription-update/). Be aware that you need the original invoice as proof of your license.
- removed support of .NET Core 3.1 because of end of life (see [here](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core))

###	Changes
- Includes all changes and fixes from the [OPC UA 1.04 Maintenance Update 1.4.371.50](https://github.com/OPCFoundation/UA-.NETStandard/releases/tag/1.4.371.50).
