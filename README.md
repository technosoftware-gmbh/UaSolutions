# Technosoftware GmbH OPC UA Solutions .NET

The OPC UA Solutions .NET are based on the OPC Foundation stack with several enhancements to make the development smooth and efficient. While we use these solutions mainly in our own services it is also possible to use it in your own development. 

## OPC UA Client .NET

Develop OPC UA compliant Clients with C# targeting .NET 10.0, .NET 9.0 and .NET 8.0. For backward compatibility we also provide .NET 4.8 and .NET 4.7.2 support.

.NET 10.0, .NET 9.0 and .NET 8.0 allows you develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10 (including embedded/IoT editions) without requiring platform-specific modifications.

The OPC UA Client .NET API defines classes which can be used to implement an OPC client capable to access OPC servers supporting different profiles with the same API. These classes manage client side state information; provide higher level abstractions for OPC tasks such as managing sessions and subscriptions or saving and restoring connection information for later use.

## OPC UA Server .NET

The OPC UA Server .NET offers a fast and easy access to the OPC Unified Architecture (UA) technology.

Develop OPC UA compliant Servers with C# targeting .NET 10.0, .NET 9.0 or .NET 8.0. For backward compatibility we also provide .NET 4.8 and .NET 4.7.2 support.

.NET 10.0, .NET 9.0 or .NET 8.0 allows you develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10 (including embedded/IoT editions) without requiring platform-specific modifications.

The OPC UA Server .NET API is easy to use and many OPC specific functions are handled by the framework. The included Model Compiler can be used to create the necessary C# classes of Information Model’s specified in XML and CSV based files. 
 
## Supported Frameworks

### V5.0 Develeopment Version (DEV)
 * .NET 10.0, .NET 9.0, .NET 8.0
 * .NET 4.8,.NET 4.7.2

### V4.2 Mainline Version (ML)
 * .NET 9.0, .NET 8.0
 * .NET 4.8,.NET 4.7.2
 
### V3.4 Long Term Support (LTS)
 * .NET 9.0, .NET 8.0
 * .NET 4.8,.NET 4.7.2

# Getting Started / Online Help
 
We provide an online help for download as ZIP file for the following versions: 

 * [V5.0 Development Version (DEV)](./help/UaSolutionsHelp50.zip)
 * [V4.2 Mainline Version (ML)](./help/UaSolutionsHelp42.zip)
 * [V3.4 Long Term Support (LTS)](./help/UaSolutionsHelp34.zip)

## Workshop

You can also use the PDFs provided from our Workshop available [here](./Workshop).

## Redistributable

- The Redistributable of the OPC UA Local Discovery Server are available [here](https://opcfoundation.org/developer-tools/samples-and-tools-unified-architecture/local-discovery-server-lds/).
- We used the version 1.04.405 for our tests

# Purchasing

## Evaluation license

The evaluation allows a smooth and efficient development of OPC UA Client and OPC UA Server applications. 

Without an evaluation license the run-time of an application using the solutions provided here is restricted to 5 miniutes. 

An evaluation license is available from:

 * [UA Solutions Evaluation](https://technosoftware.com/contact/)

Included in the evaluation are:

 * includes the license for the Mainline Version
 * Evaluation is bound to a specific version, e.g. 4.2.0 and can be used for 90 days. The run-time is restricted to 2 hours per application start.
 * Questions, Change Requests and Issues can be submitted [here](https://github.com/technosoftware-gmbh/UaSolutions/issues) free of charge.

### Support Subscriptions

Ideal for a corporation and a user of Technosoftware GmbH’s solutions on productive systems for which you want to get support for.

Included in the support subscription:

 * includes support for the the Mainline Version.
 * Any application developed with the solutions can be delivered to an unlimited number of customers (no royalties)
 * The license is bound to a specific main version, e.g. 4.
 * All Updates and fixes during the support subscription period are delivered here free of charge, if/when they are made available. Also updates to new main versions, e.g. 5.
 * You can request hotfixes for customer specific issues.
 * Questions, Change Requests and Issues can be submitted [here](https://github.com/technosoftware-gmbh/UaSolutions/issues) free of charge.
 * Also included is technical support via direct Email contact or remote sessions.
 * It is also possible to hire us for consulting on a weekly base. 

By purchasing support, you agree to our [Support Services Agreement](https://technosoftware.com/s/Support_Services_Agreement.pdf).

The Support Subscription is available [here](https://technosoftware.com/product/opc-ua-support-subscription/).

# Product Lifecycle

Version 4 is the mainline version.

Version 3.4 entered the long term support phase on 12-SEP-2025. Therefore all versions before are End of Life (EOL).

## Mainline Version (ML)

The Mainline is the most current version of a product. It’s the version we actively develop, enhance, and maintain. All new features, improvements, and components are added here. As a new customer, you’ll always receive the latest mainline version. Ordering of one of the following products/services is always for the latest mainline version:

 * [OPC UA Support Subscription](https://technosoftware.com/product/opc-ua-support-subscription/)

## Long Term Support (LTS)

When a new mainline release comes out, the version just before it enters Long Term Support (LTS). For example, if version 4.0.0 is released, the 4.0.x series becomes the mainline, while the latest 3.x.x moves into LTS.

During the LTS phase:

 * No new features are added
 * Bug fixes are provided if needed
 * The standard LTS period lasts one year (365 days) starting from the release of the next mainline version
	
This gives customers with a valid [OPC UA Support Subscription](https://technosoftware.com/product/opc-ua-support-subscription/) at least one year of transition time to update your application.

Customers with a valid [OPC UA Support Subscription](https://technosoftware.com/product/opc-ua-support-subscription/) at the time a version enters the LTS phase can request support and/or bug fixes for the LTS version. All other customers needs to upgrade to the ML version first. 
New purchases of a [OPC UA Support Subscription](https://technosoftware.com/product/opc-ua-support-subscription/) are always for the ML version.

## End of Life (EOL)

When a product’s LTS period ends, it enters End of Life (EOL). From that point on, no further updates, fixes, or support are provided.

If your application relies on a version that is nearing EOL, we strongly recommend upgrading to the latest mainline release. If upgrading is not possible, you may choose the source code edition of the [OPC UA Support Subscription](https://technosoftware.com/product/opc-ua-support-subscription/) product, which allows you to maintain and fix it yourself.

## Build Status

[![Build and Test .NET 9.0](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/buildandtest.yml/badge.svg)](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/buildandtest.yml)

## Code Quality

[![CodeQL](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/codeql.yml/badge.svg)](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/codeql.yml)
[![codecov](https://codecov.io/gh/technosoftware-gmbh/UaSolutions/graph/badge.svg?token=M927HKMQ3B)](https://codecov.io/gh/technosoftware-gmbh/UaSolutions)
