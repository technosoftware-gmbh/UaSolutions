# Node Classes

OPC UA defines the NodeClasses used to define Nodes in the OPC UA AddressSpace. NodeClasses are derived from a common, Base NodeClass. This NodeClass is defined first, followed by those used to organise the AddressSpace and then by the NodeClasses used to represent Objects.

The NodeClasses defined to represent Objects fall into three categories: those used to define instances, those used to define types for those instances and those used to define data types. This chapter gives an introduction in the mappings from OPC UA related objects to .NET classes. As specified in [OPC 10000-3] the following NodeClasses exists:

 - Base NodeClass (NodeState in .NET)
 - ReferenceType NodeClass (ReferenceTypeState in .NET)
 - View NodeClass (ViewState in .NET)
 - Object NodeClass (BaseObjectState in .NET)
 - ObjectType NodeClass (BaseObjectTypeState in .NET)
 - Variable NodeClass (BaseVariableState in .NET)
 - VariableType NodeClass (BaseVariableTypeState in .NET)
 - Method NodeClass (MethodState in .NET)

## Base NodeClass

The OPC UA Address Space Model defines a Base NodeClass from which all other NodeClasses are derived. The derived NodeClasses represent the various components of the OPC UA Object Model. The Attributes of the Base NodeClass are specified in [OPC 10000-3] of the OPC UA specification. There are no References specified for the Base NodeClass. The [Base NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.2) is represented by the [NodeState class](#Opc.Ua.NodeState)) in .NET.

## ReferenceType NodeClass

References are defined as instances of ReferenceType Nodes. ReferenceType Nodes are visible in the AddressSpace and are defined using the [ReferenceType NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.3) and represented in .NET using the [ReferenceTypeState class](#Opc.Ua.ReferenceTypeState). In contrast, a Reference is an inherent part of a Node and no NodeClass is used to represent References. 

This standard defines a set of ReferenceTypes provided as an inherent part of the OPC UA Address Space Model. These ReferenceTypes are defined in [Clause 7](https://reference.opcfoundation.org/Core/Part3/v105/docs/7#_Ref192918160) and their representation in the AddressSpace is defined in [OPC 10000-5]. Servers may also define ReferenceTypes. In addition, [OPC 10000-4] defines NodeManagement Services that allow Clients to add ReferenceTypes to the AddressSpace

## View NodeClass

Underlying systems are often large, and Clients often have an interest in only a specific subset of the data. They do not need, or want, to be burdened with viewing Nodes in the AddressSpace for which they have no interest. 

To address this problem, this standard defines the concept of a View. Each View defines a subset of the Nodes in the AddressSpace. The entire AddressSpace is the default View. Each Node in a View may contain only a subset of its References, as defined by the creator of the View. The View Node acts as the root for the Nodes in the View. Views are defined using the [View NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.4) and represented by the [ViewState class](#Opc.Ua.ViewState) in .NET.

All Nodes contained in a View shall be accessible starting from the View Node when browsing in the context of the View. It is not expected that all containing Nodes can be browsed directly from the View Node but rather browsed from other Nodes contained in the View. 

A View Node may not only be used as additional entry point into the AddressSpace but as a construct to organize the AddressSpace and thus as the only entry point into a subset of the AddressSpace. Therefore, Clients shall not ignore View Nodes when exposing the AddressSpace. Simple Clients that do not deal with Views for filtering purposes can, for example, handle a View Node like an Object of type FolderType

## Objects

### Objects NodeClass

Objects are used to represent systems, system components, real-world objects and software objects. Objects are defined using the [Object NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.5) and represented in .NET using the [BaseObjectState class](#Opc.Ua.BaseObjectState).

### ObjectType NodeClass

ObjectTypes provide definitions for Objects. ObjectTypes are defined using the [ObjectType NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.5.2) and represented in .NET using the [BaseObjectTypeState class](#Opc.Ua.BaseObjectTypeState).

## Variables

Variables are used to represent values. Two types of Variables are defined, Properties and DataVariables. They differ in the kind of data that they represent and whether they can contain other Variables.

### Variable NodeClass

Variables are used to represent values which may be simple or complex. Variables are defined by VariableTypes. 

Variables are always defined as Properties or DataVariables of other Nodes in the AddressSpace. They are never defined by themselves. A Variable is always part of at least one other Node but may be related to any number of other Nodes. Variables are defined using the [Variable NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.6.2) and represented in .NET using the [BaseVariableState class](#Opc.Ua.BaseDataVariableState).

### VariableType NodeClass

VariableTypes are used to provide type definitions for Variables. VariableTypes are defined using the [VariableType NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.6.5) and represented in .NET using the [BaseVariableTypeState class](#Opc.Ua.BaseDataVariableTypeState).

## Method NodeClass

Methods define callable functions. Methods are invoked using the Call Service defined in [OPC 10000-4]. Method invocations are not represented in the AddressSpace. Method invocations always run to completion and always return responses when complete. Methods are defined using the [Method NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.7.1) and represented in .NET using the [MethodState class](#Opc.Ua.MethodState).

## DataTypes

### DataType NodeClass

The DataType NodeClass describes the syntax of a Variable Value. DataTypes are defined using the [DataType NodeClass](https://reference.opcfoundation.org/Core/Part3/v105/docs/5.8.3) and represented in .NET using the [DataTypeState class](#Opc.Ua.DataTypeState).


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