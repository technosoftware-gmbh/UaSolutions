# Develop OPC UA Servers with C\# for of .NET

## OPC UA Server .NET

The OPC UA Server NET offers a fast and easy access to the OPC Unified Architecture (UA) technology.

Develop OPC UA compliant Servers with C# targeting .NET 9.0 or .NET 8. For backward compatibility we also provide .NET 4.8 and .NET 4.7.2 support.

.NET 9.0 or .NET 8 allows you develop applications that run on all common platforms available today, including Linux, macOS and Windows 11/10 (including embedded/IoT editions) without requiring platform-specific modifications.

The server API is easy to use and many OPC specific functions are handled by the framework. The included Model Compiler can be used to create the necessary C# classes of Information Model’s specified in XML and CSV based files. 

Documentation of the Model Compiler can be found [here](https://github.com/OPCFoundation/UA-ModelCompiler).

**Document Control**

| **Version** | **Date**    | **Comment**                          |
|-------------|-------------|--------------------------------------|
| 3.1         | 28-APR-2023 | Initial version based on version 3.1 |
| 3.3         | 02-FEB-2024 | Updated to new sample server         |
| 4.0         | 02-APR-2025 | Initial version based on version 4.0 |

**Purpose and audience of document**

Microsoft’s .NET is an application development environment that supports multiple languages and provides a large set of standard programming APIs. This document defines an Application Programming Interface (API) for OPC UA Client development based on the .NET programming model.

This document gives a short overview of the functionality of the OPC UA .NET solutions family. The goal of this document is to give an introduction and can be used as base for your own implementations.

**Referenced OPC Documents**

| **Documents**                                                                                                                                                                                                                             |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Online versions of OPC UA specifications and information models. The OPC UA Online Reference is available at:  <https://reference.opcfoundation.org>                                                                                      |
| OPC Unified Architecture Textbook, written by Wolfgang Mahnke, Stefan-Helmut Leitner and Matthias Damm:  <http://www.amazon.com/OPC-Unified-Architecture-Wolfgang-Mahnke/dp/3540688986/ref=sr_1_1?ie=UTF8&s=books&qid=1209506074&sr=8-1>  |


## [Supported OPC UA Profiles](./UaServer/SupportedProfiles.md)

## [Sample Application](./UaServer/SampleApplication.md)

## [Server Design based opn UaBaseServer](./UaServer/UaBaseServerDesign.md)