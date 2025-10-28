# OPC UA Solutions .NET {#mainpage}

[TOC]

## Overview

If your application requires access to the OPC technology, you must decide which of the OPC Specifications you want to support. The decision depends on many factors like the used OPC specifications supported by third party solutions or the system architecture you want to use. Technosoftware GmbH specializes in software consulting and development services for technical and industrial applications based on OPC and the founder of Technosoftware GmbH was involved in the design and development of many software solutions with OPC connectivity with more than 22 years extensive knowledge about OPC.

The OPC Unified Architecture (UA) is **THE** next generation OPC standard that provides a cohesive, secure, and reliable cross platform framework for access to real time and historical data and events.

**It's time to adapt** this specification for use in your applications, keeping in mind that you may need to support other OPC specifications as well. The Classic OPC specifications are widely used, and it is important that your application can support these specifications. We recommend that you design your application to use the OPC Unified Architecture in the first place, with the option to have access to the Classic OPC Specifications in the second place. A short overview of the mainly used Classic OPC Specifications should give you an understanding of the requirements your application design has to fulfill.

### OPC Technology

#### Classic OPC Specifications

The Classic OPC specifications are DCOM based specifications like OPC Data Access (DA), OPC Alarms&Events (AE) and OPC Historical Access (HDA). Each of these specifications is a separate specification and several more exist.

According to the different requirements within industrial applications, three majors Classic OPC specifications have been developed. Support of one or more of these specifications should be provided by most of the OPC Client or OPC Server applications.

##### Data Access (DA)

An OPC DA Server allows OPC DA Clients to retrieve information about several objects: the server, the group and the items.  
 - The OPC server object maintains information about the server and acts as a container for OPC group objects. 
 - The OPC group object maintains information about itself and provides the mechanism for containing and logically organizing OPC items. 
 - The OPC items represent connections to data sources within the server. **The OPC DA Specification defines two read/write interfaces:**
   - Synchronous  
   The client can perform a synchronous read from cache (simple and reasonably efficient). This may be appropriate for simple clients that are reading relatively small amounts of data. 
   - Asynchronous  
   The client can ‘subscribe’ to cached data using IAdviseSink or IOPCDataCallback which is more complex but very efficient. Asynchronous access is recommended because it minimizes the use of CPU and NETWORK resources.  
   
In all cases the OPC DA Server gives the client access to current values of the OPC items. The OPC DA Server only holds current information in cache. Old information is overwritten. As a result of this it cannot be guaranteed that an OPC DA Client retrieves all changes in values (also not in asynchronous mode).

For such cases, there exist two more OPC specifications, the OPC Alarms&Events and the OPC Historical Data Access Specification.

##### Alarms&Events (AE)

The OPC AE interface provides a mechanism for OPC AE clients to be notified when a specified event and/or alarm condition occurs. The browser interface also allows OPC AE clients to determine the list of events and conditions supported by an OPC AE Server as well as to get their status.

Within OPC, an alarm is an abnormal condition and is thus a special case of a condition.  A condition is a named state of the OPC Event Server or of one of its contained objects that is of interest to an OPC AE client.  For example, the tag Temperature may have the following conditions associated with it: HighAlarm, HighHighAlarm, Normal, LowAlarm, and LowLowAlarm.

On the other hand, an event is a detectable occurrence that is of significance to the OPC Server, the device it represents, and its OPC AE clients.  An event may or may not be associated with a condition.  For example, the transition into HighAlarm and Normal conditions are events, which are associated with conditions.  However, operator actions, system configuration changes, and system errors are examples of events which are not related to specific conditions.  OPC AE clients may subscribe to being notified of the occurrence of specified events. 

The OPC AE specification provides methods enabling the OPC AE client to:
 - Determine the types of events that are supported by the OPC AE server.
 - Enter subscriptions to specified events so that OPC AE clients can receive notifications of their occurrences. Filters may be used to define a subset of desired events.
 - Access and manipulate conditions implemented by the OPC AE server.

##### Historical Data Access (HDA)

Historical engines today produce an added source of information that should be distributed to users and software clients that are interested in this information. Currently most historical systems use their own proprietary interfaces for dissemination of data. There is no capability to augment or use existing historical solutions with other capabilities in a plug-and-play environment. This requires the developer to recreate the same infrastructure for their solutions, as all other vendors have had to develop independently with no interoperability with any other systems.

In keeping with the desire to integrate data at all levels of business, historical information can be another type of data. 

There are several types of Historian servers. Some key types supported by the HDA specification are:
 - Simple Trend data servers.  
 These servers provided little else than simple raw data storage. (Data would typically be the types of data available from an OPC Data Access server, usually provided in the form of a duple [Time Value & Quality])
 - Complex data compression and analysis servers.  
 These servers provide data compression as well as raw data storage. They can provide summary data or data analysis functions, such as average values, minimums, maximums etc.  They can support data updates and the history of the updates.  They can support storage of annotations along with the actual historical data storage.
 
#### OPC XML-DA Specification

The OPC XML-DA specification is a web services-based specification and is a step between Classic OPC and OPC Unified Architecture. The functionality is restricted to OPC DA.

Technosoftware GmbH does not offer any solutions supporting the OPC XML-DA Specification.

##### OPC .NET 4.0 (WCF)

OPC .NET 4.0 (WCF) is not a specification like the others mentioned here, it bridges the gap between Microsoft.NET and the world of Classic OPC. OPC .NET 4.0 is an OPC standard C# Application Programming Interface (API) designed to simplify client access to OPC Classic servers. It also includes a formal WCF Interface; however, there is no compliance or certification program for this interface. The Classic OPC interfaces are still the primary means to ensure multi-vendor interoperability.

Technosoftware GmbH does not offer any solutions supporting the OPC .NET 4.o API. 

#### OPC Unified Architecture Specification

OPC Unified Architecture (UA) is a platform-independent standard through which various kinds of systems and devices can communicate by sending Messages between Clients and Servers over various types of networks. It supports robust, secure communication that assures the identity of Clients and Servers and resists attacks. OPC UA defines standard sets of Services that Servers may provide, and individual Servers specify to Clients what Service sets they support. Information is conveyed using standard and vendor- defined data types, and Servers define object models that Clients can dynamically discover. Servers can provide access to both current and historical data, as well as Alarms and Events to notify Clients of important changes. OPC UA can be mapped onto a variety of communication protocols and data can be encoded in various ways to trade off portability and efficiency.

The OPC Foundation provides deliverables for its member companies. These include a .NET based OPC UA Stack, an ANSI C based OPC UA Stack and a Java based OPC UA Stack. The .NET based OPC UA Stack is the base of all Technosoftware GmbH's .NET based solutions.

![](images/OPCUAServices.png)

OPC Unified Architecture (UA) is a platform-independent standard through which various kinds of systems and devices can communicate by sending Messages between Clients and Servers over various types of networks. It supports robust, secure communication that assures the identity of Clients and Servers and resists attacks. OPC UA defines standard sets of Services that Servers may provide, and individual Servers specify to Clients what Service sets they support. Information is conveyed using standard and vendor-defined data types, and Servers define object models that Clients can dynamically discover. Servers can provide access to both current and historical data, as well as Alarms and Events to notify Clients of important changes. OPC UA can be mapped onto a variety of communication protocols and data can be encoded in various ways to trade off portability and efficiency.

OPC UA provides a consistent, integrated AddressSpace and service model. This allows a single OPC UA Server to integrate data, Alarms and Events, and history into its AddressSpace, and to provide access to them using an integrated set of Services. These Services also include an integrated security model.

OPC UA also allows Servers to provide Clients with type definitions for the Objects accessed from the AddressSpace. This allows standard information models to be used to describe the contents of the AddressSpace. OPC UA allows data to be exposed in many different formats, including binary structures and XML documents. The format of the data may be defined by OPC, other standard organizations or vendors. Through the AddressSpace, Clients can query the Server for the metadata that describes the format for the data. In many cases, Clients with no pre-programmed knowledge of the data formats will be able to determine the formats at runtime and properly utilize the data.

OPC UA adds support for many relationships between Nodes instead of being limited to just a single hierarchy. In this way, an OPC UA Server may present data in a variety of hierarchies tailored to the way a set of Clients would typically like to view the data. This flexibility, combined with support for type definitions, makes OPC UA applicable to a wide array of problem domains. As illustrated below, OPC UA is not targeted at just the telemetry server interface, but also to provide greater interoperability between higher level functions.

OPC UA is designed to provide robustness of published data. A major feature of all OPC servers is the ability to publish data and Event Notifications. OPC UA provides mechanisms for Clients to quickly detect and recover from communication failures associated with these transfers without having to wait for long timeouts provided by the underlying protocols.

OPC UA is designed to support a wide range of Servers, from plant floor PLCs to enterprise Servers. These Servers are characterized by a broad scope of size, performance, execution platforms and functional capabilities. Therefore, OPC UA defines a comprehensive set of capabilities, and Servers may implement a subset of these capabilities. To promote interoperability, OPC UA defines standard subsets, referred to as Profiles, to which Servers may claim conformance. Clients can then discover the Profiles of a Server, and tailor their interactions with that Server based on the Profiles. Profiles are defined in [UA Part 7].
 
The OPC UA specifications are layered to isolate the core design from the underlying computing technology and network transport. This allows OPC UA to be mapped to future technologies as necessary, without negating the basic design. Mappings and data encodings are described in [UA Part 6]. Two data encodings are defined in this part:
 - XML/text
 - UA Binary

In addition, two transport mappings are defined in this part:
 - TCP
 - SOAP Web services over HTTP

Clients and Servers that support multiple transports and encodings will allow the end users to make decisions about tradeoffs between performance and XML Web service compatibility at the time of deployment, rather than having these tradeoffs determined by the OPC vendor at the time of product definition.

OPC UA is designed as the migration path for OPC clients and servers that are based on Microsoft COM technology. Care has been taken in the design of OPC-UA so that existing data exposed by OPC COM servers (DA, HDA and A&E) can easily be mapped and exposed via OPC UA. Vendors may choose to migrate their solutions natively to OPC UA or use external wrappers to convert from OPC COM to OPC UA and vice-versa. Each of the previous OPC specifications defined its own address space model and its own set of Services. OPC UA unifies the previous models into a single integrated address space with a single set of Services.