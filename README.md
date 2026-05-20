# Technosoftware GmbH OPC UA Solutions .NET

## Overview

The next generation of the OPC UA Solutions .NET uses the original OPC Foundation Stack and only Configuration, Client and Server libraries will be the same as for version 5 and before. 

There are only minimal breaking changes because of that which are described in the [changelog](./CHANGELOG.md).

Configuration, Client and Server libraries are provided as source code and are not compiled into a separate library. This allows you to easily modify the code if needed and also to use the latest OPC Foundation Stack without having to wait for us to update it.

Develop OPC UA compliant Clients and Servers with C# targeting .NET 10.0. 

.NET 10.0 allows you develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10 (including embedded/IoT editions) without requiring platform-specific modifications.

## Important

Version 6.0 has the same functionality as version 5.0 and uses also the same OPC Foundation Stack Version 1.5.378.134. The only differences are that version 6.0 only supports .NET 10.0 and uses the original OPC Foundation Stack instead of a slightly modified version.

If you need support for .NET 9.0, .NET 8.0, .NET 4.8, .NET 4.72 or want to use the UA Client Gateway please use version 5. You find them in the subdirectory [legacy](./legacy). Version 5 will be supported for customers with license and valid support subscription until 31-MAY-2027.

## Documentation

The documentation is available [here](https://technosoftware-gmbh.github.io/UaSolutions/).

## Changelog

A changelog is available [here](./CHANGELOG.md).

## Purchasing

### Product license

Starting with version 6, the OPC UA Solutions .NET are available under a MIT license. We no longer offer licenses for the use of the software. If you want to use the software without support, you can simply clone the repository and use it under the MIT license.

### Support

Contact us about the availability of Support [here](https://technosoftware.com/contact).

## Product Lifecycle

Our products, especially software, go through different stages during their lifetime. To make this process straightforward, we follow a clear lifecycle strategy. Each time a new release (a new minor or major version) is published, it becomes the active development line—we call this the Mainline. The previous version then moves into Long Term Support (LTS), where it continues to receive bug fixes for a limited time. Eventually, products reach End of Life (EOL), which means support for them ends.

### Support Dates

| **Version** | **Current State** | **Retirement Date**     | **Available Support**                                                         |
|:------------|:------------------|:------------------------|:------------------------------------------------------------------------------|
| 3.4         | EOL               | 31-MAY-2026             |                                                                               |
| 4.3         | EOL               | 31-MAY-2026             |                                                                               |
| 5.0         | LTS               | 31-MAY-2027, In Support | Yes, with a license and valid support subscription                            |
| 6.0         | ML                | In Support              | Yes, with a valid support subscription                                        |

Version 6 will enter the mainline version phase on 31-MAY-2026.

Version 5 is the current mainline version but will enter the long term support phase on 31-MAY-2026. This version will be supported for customers with license and valid support subscription at least until 31-MAY-2027.

Because of too many breaking changes in the OPC Foundationn stack the following versions can no longer be maintained and are end-of-life from 31-MAY-2026 on:

 * Version 4.x
 * Version 3.4 

It is highly recommended to update to version 5 or 6 now.

### Mainline Version (ML)

The Mainline is the most current version of a product. It’s the version we actively develop, enhance, and maintain. All new features, improvements, and components are added here. As a new customer, you’ll always receive the latest mainline version. The support subscription always includes the latest mainline version.

 * [Get in contact with us](https://technosoftware.com/contact)

### Long Term Support (LTS)

When a new mainline release comes out, the version just before it enters Long Term Support (LTS). For example, if version 6 is released, the 6.x series becomes the mainline, while the latest 5.x moves into LTS.

During the LTS phase:

 * No new features are added
 * Bug fixes are provided if needed
 * The standard LTS period lasts one year (365 days) starting from the release of the next mainline version
	
This gives customers with a valid support subscription at least one year of transition time to update your application.

Customers with a valid support subscription at the time a version enters the LTS phase can request support and/or bug fixes for the LTS version. All other customers needs to upgrade to the ML version first. New purchases of a support subscription are always for the ML version.

### End of Life (EOL)

When a product’s LTS period ends, it enters End of Life (EOL). From that point on, no further updates, fixes, or support are provided.

If your application relies on a version that is nearing EOL, we strongly recommend upgrading to the latest mainline release. If upgrading is not possible, you may request a quote for the source code edition of the solution, which allows you to maintain and fix it yourself.

## Contributing

We strongly encourage community participation and contribution to this project. First, please fork the repository and commit your changes there. Once happy with your changes you can generate a 'pull request'.

You must agree to the contributor license agreement before we can accept your changes. The CLA and "I AGREE" button is automatically displayed when you perform the pull request. You can preview CLA [here](https://cla-assistant.io/technosoftware-gmbh/UaSolutions).

## Status

[![Build and Test .NET 10.0](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/buildandtest.yml/badge.svg)](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/buildandtest.yml) [![CodeQL](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/github-code-scanning/codeql) [![codecov](https://codecov.io/gh/technosoftware-gmbh/UaSolutions/graph/badge.svg?token=AU2E8V3MKV)](https://codecov.io/gh/technosoftware-gmbh/UaSolutions) [![Github top language](https://img.shields.io/github/languages/top/technosoftware-gmbh/UaSolutions)](https://github.com/technosoftware-gmbh/UaSolutions)
