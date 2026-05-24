# Technosoftware GmbH OPC UA Solutions .NET

## Overview

The next generation of the OPC UA Solutions .NET uses the original OPC Foundation Stack and only Configuration, Client and Server libraries will be the same as for version 5 and before. 

There are only minimal breaking changes because of that which are described in the [changelog](https://technosoftware-gmbh.github.io/UaSolutions/changelog/index.html).

Configuration, Client and Server libraries are provided as source code and are not compiled into a separate library. This allows you to easily modify the code if needed and also to use the latest OPC Foundation Stack without having to wait for us to update it.

Develop OPC UA compliant Clients and Servers with C# targeting .NET 10.0. 

.NET 10.0 allows you develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10 (including embedded/IoT editions) without requiring platform-specific modifications.

## Important

Version 6.0 has the same functionality as version 5.0 and uses also the same OPC Foundation Stack Version 1.5.378.134. The only differences are that version 6.0 only supports .NET 10.0 and uses the original OPC Foundation Stack instead of a slightly modified version.

If you need support for .NET 9.0, .NET 8.0, .NET 4.8, .NET 4.72 or want to use the UA Client Gateway please use version 5. You find them in the subdirectory [legacy](https://github.com/technosoftware-gmbh/UaSolutions/tree/main/legacy). Version 5 will be supported for customers with license and valid support subscription until 31-MAY-2027.

## Documentation

The documentation is available [here](https://technosoftware-gmbh.github.io/UaSolutions/).

## Changelog

A changelog is available [here](https://technosoftware-gmbh.github.io/UaSolutions/changelog/index.html).

## Purchasing

### Product license

Starting with version 6, the OPC UA Solutions .NET are available under a MIT license. We no longer offer licenses for the use of the software. If you want to use the software without support, you can simply clone the repository and use it under the MIT license.

### Support

Contact us about the availability of Support [here](https://technosoftware.com/contact).

## Product Lifecycle

Our products, especially software, go through different stages during their lifetime. To make this process straightforward, we follow a clear [lifecycle strategy](https://technosoftware-gmbh.github.io/UaSolutions/lifecycle/).

## Contributing

We strongly encourage community participation and contribution to this project. First, please fork the repository and commit your changes there. Once happy with your changes you can generate a 'pull request'.

You must agree to the contributor license agreement before we can accept your changes. The CLA and "I AGREE" button is automatically displayed when you perform the pull request. You can preview CLA [here](https://cla-assistant.io/technosoftware-gmbh/UaSolutions).

## Status

[![Build and Test .NET 10.0](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/buildandtest.yml/badge.svg)](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/buildandtest.yml) [![CodeQL](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/technosoftware-gmbh/UaSolutions/actions/workflows/github-code-scanning/codeql) [![codecov](https://codecov.io/gh/technosoftware-gmbh/UaSolutions/graph/badge.svg?token=AU2E8V3MKV)](https://codecov.io/gh/technosoftware-gmbh/UaSolutions) [![Github top language](https://img.shields.io/github/languages/top/technosoftware-gmbh/UaSolutions)](https://github.com/technosoftware-gmbh/UaSolutions)
