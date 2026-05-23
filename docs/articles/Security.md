# Security

This chapter is based on the [Whitepaper The OPC UA Security Model for Administrators, Version 1.00 from July, 2010](https://opcfoundation.org/wp-content/uploads/2014/05/OPC-UA_Security_Model_for_Administrators_V1.00.pdf) and should be used as reference. 

## Background

![](../images/CertificateHandling.png)

A security model is an architecture that allows developers, administrators and end users to use applications in a distributed environment while ensuring that the applications, the computers they run on, and the information exchanged is not compromised.  A complete security model has several facets including application security, transport security, user authorization and authentication and traceability.  This white paper describes how to use the OPC UA security model to ensure application and transport security. The target audience for this document are systems administrators and end users. A second whitepaper will discuss the security model from the perspective of a software developer. 

The OPC UA security model has been designed to meet the requirements of many different systems while using the same infrastructure. To accommodate different security and administrative requirements the OPC UA security model offers four tiers for application authentication and two tiers for certificate management. It is up to the administrator to decide which tiers best match their needs. Applications should support all tiers. This document also discusses the administrative procedures required by a tier. Applications must allow administrators to configure the level of security enforced by their application just like web browsers allow administrators to configure the security level enforced by the browser.

The UA Security Model defines four principal actors: The Application Instance, the Application Administrator, the Application Operator and the Certificate Authority. The relationships between these actors are shown in the following figure. Each of the entities is described in the text that follows.  

### Security Tiers 

#### The Basics 

In OPC UA, each installation of an application must have an application instance certificate that uniquely identifies the application and the machine that it is running on. These certificates come with private keys that allow applications to create secure communication channels that cannot be viewed by 3rd parties or modified while in transit. These certificates also allow OPC UA applications to be identified by peers and to block communication from a peer if it is not authorized.   

#### Tier 1 - No Authentication

In this tier the client and server allow any peer to communicate which means that all valid certificates are trusted. The application certificates are used only to provide unverifiable information about the peer. The receiver has no way to know if the sender is the legitimate holder of the certificate. 

In this mode the client and server automatically accept valid certificates even if they have not been explicitly added to the trust lists managed by the client and server applications. This mode requires no configuration at the server or client. 

This tier cannot ensure the privacy of any information transmitted, including user credentials. This tier would only be appropriate in a system that has guaranteed security in some other manner, such as a physically secured and isolated system or where communications is secured via VPN or other such transport layer security.  It would also be appropriate in a situation where all information is public and access to it is open. 

A developer need only configure an installation procedure that generates an application instance certificate for an application on installation. 

#### Tier 2 - Server Authentication

In this tier the server allows any client to connect and if user authentication is required it is done by sending user credentials such as a username/password after the secure channel has been established. Clients, on the other hand, must be configured by the administrator to trust the Server.  

Clients will trust a Server if an administrator has explicitly placed the Server certificate into its trust list or if the Server's certificate was issued by a CA which is in its trust list. 

If the Server's certificate was not explicitly in the trust list (i.e. the certificate was issued by a CA that is in the trust list) the client should compare the DNS name in the Server's certificate to the DNS name it used to connect to server. If they match the client knows it is connecting to the machine it thinks it is connecting to.  This does not guarantee that the client has connected to correct server, only that the machine is the correct machine.   

This tier is used by most Internet banking applications where the bank's web server has a certificate issued by Certificate Authorities like Verisign which are automatically placed in the browsers trust list by the Windows operating system. It provides a fairly good security, but the server cannot restrict the client applications. 
 
#### Tier 3 - Client Authentication

In this tier the client connects to any server, but the server only allows trusted clients to connect. Clients never provide sensitive information since it does not know if it the server is legitimate. 

In this tier clients need no pre-configuration other than the URL of the server. However, Servers will only trust clients with certificates that have been placed by administrators in the server’s trust list or if the Clients certificate was issued by a CA which is in its trust list 

This mode is used by discovery services which need to ensure that only authorized applications have access to them, but clients don't care if the server is not legitimate. The local discovery server (LDS) operates in this mode and only allows authorized applications to register themselves.

#### Tier 4 - Mutual Authentication

In this tier both the client and server only allow trusted peers to connect. It offers the highest level of security but requires that both the client and server be configured in advance. This is the recommended mode for any public or semi-public deployment of OPC UA or for deployments where security is a primary concern. 

As in Tier 2, clients should check the DNS name if the Server certificate was not explicitly placed in the trust list. 

It will be used in environments where administrators want complete control over which applications can talk to each other. It also provides the most secure environment. 

Application installation should default to Tier 4 mode

[OPC 10000-1]: https://reference.opcfoundation.org/Core/Part1/v105/docs/
[OPC 10000-2]: https://reference.opcfoundation.org/Core/Part2/v105/docs/
[OPC 10000-3]: https://reference.opcfoundation.org/Core/Part3/v105/docs/
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