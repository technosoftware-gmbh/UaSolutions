# Basics

[TOC]

This chapter describes the base concepts of the OPC UA specification. It is not intended to replace studying the OPC UA specification; its purpose is to introduce the base concepts of the OPC UA specification.

## Overview

### Specifications

The OPC UA specification is organized as a multi-part specification, splitted into 3 sections:

 - Core Specification Parts  
   The first seven parts specify the core capabilities of OPC UA. These core capabilities define the structure of the OPC AddressSpace and the Services that operate on it. 
	
 - Access Type Specification Parts  
   [OPC 10000-8], [OPC 10000-9], [OPC 10000-10] and [OPC 10000-11] apply these core capabilities to specific types of access previously addressed by separate OPC COM specifications, such as Data Access (DA), Alarms and Events (A&E) and Historical Data Access (HDA). 
	
 - Utility Specification Parts  
   [OPC 10000-12]: describes Discovery mechanisms for OPC UA and [OPC 10000-13](https://reference.opcfoundation.org/Core/Part13/v105/docs/) describe ways of aggregating data. [OPC 10000-14](https://reference.opcfoundation.org/Core/Part14/v105/docs/) defines the PubSub communication model. The PubSub communication model defines an OPC UA publish subscribe pattern instead of the client server pattern defined by the Services in Part 4.
   
The OPC UA specification is also available as online reference [here](https://reference.opcfoundation.org/). 

The following sections give the reader an introduction in OPC UA. Most of these parts are summaries of the OPC UA specifications and are reduced to those parts which are required for the reader to understand the basics of OPC UA. Mappings between definitions in the OPC UA specifications and those used in the OPC UA .NET Standard Solutions are given as soon as they are used the first time.

### Graphical Notation

The OPC UA specification [OPC 10000-3 Annex C] defines a graphical notation for an OPC UA AddressSpace. It defines graphical symbols for all NodeClasses and how References of different types can be visualized. This notation is also used in this documentation and therefore it is recommended to have an understanding of it.

### Security

The OPC UA specification [OPC 10000-2] describes the OPC Unified Architecture (OPC UA) security model.

### Address Space

The OPC UA specification [OPC 10000-3] describes the OPC Unified Architecture (OPC UA) AddressSpace and its Objects.

### Services

The OPC UA specification [OPC 10000-4] describes the OPC Unified Architecture (OPC UA) Services.

### Information Model

The OPC UA specification [OPC 10000-5] describes the OPC Unified Architecture (OPC UA) Information Model.

### Mappings

The OPC UA specification [OPC 10000-6] specifies the mapping between the security model described in [OPC 10000-2], the abstract service definitions specified in [OPC 10000-4], the data structures defined in [OPC 10000-5] and the physical network protocols that can be used to implement the OPC UA specification.

### Profiles

The OPC UA specification [OPC 10000-7] specifies value and structure of Profiles in the OPC Unified Architecture.

### Data Access

The OPC UA specification [OPC 10000-8] is part of the overall OPC Unified Architecture (OPC UA) standard series and defines the information model associated with Data Access (DA).

### Alarms and Conditions

The OPC UA specification [OPC 10000-9] specifies the representation of Alarms and Conditions in the OPC Unified Architecture. Included is the Information Model representation of Alarms and Conditions in the OPC UA address space.

### Programs

The OPC UA specification [OPC 10000-10] defines the Information Model associated with Programs in OPC Unified Architecture (OPC UA). This includes the description of the NodeClasses, standard Properties, Methods and Events and associated behaviour and information for Programs.

### Historical Access

The OPC UA specification [OPC 10000-11] defines the Information Model associated with Historical Access (HA). It particularly includes additional and complementary descriptions of the NodeClasses and Attributes needed for Historical Access, additional standard Properties, and other information and behaviour.

### Discovery and Global Services

The OPC UA specification [OPC 10000-12] specifies how OPC Unified Architecture (OPC UA) Clients and Servers interact with DiscoveryServers when used in different scenarios. It specifies the requirements for the LocalDiscoveryServer, LocalDiscoveryServer-ME and GlobalDiscoveryServer. It also defines information models for Certificate management, KeyCredential m anagement and AuthorizationServices.

### Aggregates

The OPC UA specification [OPC 10000-13] defines the information model associated with Aggregates.

### PubSub

The OPC UA specification [OPC 10000-14] defines the information model associated with Aggregates.

[OPC 10000-1]: https://reference.opcfoundation.org/Core/Part1/v105/docs/
[OPC 10000-2]: https://reference.opcfoundation.org/Core/Part2/v105/docs/
[OPC 10000-3]: https://reference.opcfoundation.org/Core/Part3/v105/docs/
[OPC 10000-3 AddressSpace]: https://reference.opcfoundation.org/Core/Part3/v105/docs/4
[OPC 10000-3 Annex C]: https://reference.opcfoundation.org/Core/Part3/v105/docs/C
[OPC 10000-4]: https://reference.opcfoundation.org/Core/Part4/v105/docs/
[OPC 10000-5]: https://reference.opcfoundation.org/Core/Part5/v105/docs/
[OPC 10000-6]: https://reference.opcfoundation.org/Core/Part6/v105/docs/
[OPC 10000-7]: https://reference.opcfoundation.org/Core/Part7/v105/docs/
[OPC 10000-8]: https://reference.opcfoundation.org/Core/Part8/v105/docs/
[OPC 10000-9]: https://reference.opcfoundation.org/Core/Part9/v105/docs/
[OPC 10000-10]: https://reference.opcfoundation.org/Core/Part10/v105/docs/
[OPC 10000-11]: https://reference.opcfoundation.org/Core/Part11/v105/docs/
[OPC 10000-12]: https://reference.opcfoundation.org/Core/Part12/v105/docs/
[OPC 10000-13]: https://reference.opcfoundation.org/Core/Part13/v105/docs/
[OPC 10000-14]: https://reference.opcfoundation.org/Core/Part14/v105/docs/

