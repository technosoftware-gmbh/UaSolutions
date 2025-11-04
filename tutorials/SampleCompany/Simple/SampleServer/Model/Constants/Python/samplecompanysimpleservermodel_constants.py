from enum import Enum

class Namespaces(Enum):
     Uri = "http://samplecompany.com/SimpleServer/Model"

class BrowseNames(Enum):
    CurrentStep = "CurrentStep"
    CycleId = "CycleId"
    CycleStepDataType = "CycleStepDataType"
    Error = "Error"
    SimpleServer_BinarySchema = "SampleCompany.SimpleServer.Model"
    SimpleServer_XmlSchema = "SampleCompany.SimpleServer.Model"
    Steps = "Steps"
    SystemCycleAbortedEventType = "SystemCycleAbortedEventType"
    SystemCycleFinishedEventType = "SystemCycleFinishedEventType"
    SystemCycleStartedEventType = "SystemCycleStartedEventType"
    SystemCycleStatusEventType = "SystemCycleStatusEventType"

class DataTypeIds(Enum):
    CycleStepDataType = "nsu=http://samplecompany.com/SimpleServer/Model;i=1"

def get_DataTypeIds_name(value: str) -> str:
    try:
        return DataTypeIds(value).name
    except ValueError:
        return None


class ObjectIds(Enum):
    CycleStepDataType_Encoding_DefaultBinary = "nsu=http://samplecompany.com/SimpleServer/Model;i=52"
    CycleStepDataType_Encoding_DefaultXml = "nsu=http://samplecompany.com/SimpleServer/Model;i=60"
    CycleStepDataType_Encoding_DefaultJson = "nsu=http://samplecompany.com/SimpleServer/Model;i=68"

def get_ObjectIds_name(value: str) -> str:
    try:
        return ObjectIds(value).name
    except ValueError:
        return None


class ObjectTypeIds(Enum):
    SystemCycleStatusEventType = "nsu=http://samplecompany.com/SimpleServer/Model;i=2"
    SystemCycleStartedEventType = "nsu=http://samplecompany.com/SimpleServer/Model;i=14"
    SystemCycleAbortedEventType = "nsu=http://samplecompany.com/SimpleServer/Model;i=27"
    SystemCycleFinishedEventType = "nsu=http://samplecompany.com/SimpleServer/Model;i=40"

def get_ObjectTypeIds_name(value: str) -> str:
    try:
        return ObjectTypeIds(value).name
    except ValueError:
        return None


class VariableIds(Enum):
    SystemCycleStatusEventType_CycleId = "nsu=http://samplecompany.com/SimpleServer/Model;i=12"
    SystemCycleStatusEventType_CurrentStep = "nsu=http://samplecompany.com/SimpleServer/Model;i=13"
    SystemCycleStartedEventType_Steps = "nsu=http://samplecompany.com/SimpleServer/Model;i=26"
    SystemCycleAbortedEventType_Error = "nsu=http://samplecompany.com/SimpleServer/Model;i=39"
    SimpleServer_BinarySchema = "nsu=http://samplecompany.com/SimpleServer/Model;i=53"
    SimpleServer_BinarySchema_NamespaceUri = "nsu=http://samplecompany.com/SimpleServer/Model;i=55"
    SimpleServer_BinarySchema_Deprecated = "nsu=http://samplecompany.com/SimpleServer/Model;i=56"
    SimpleServer_BinarySchema_CycleStepDataType = "nsu=http://samplecompany.com/SimpleServer/Model;i=57"
    SimpleServer_XmlSchema = "nsu=http://samplecompany.com/SimpleServer/Model;i=61"
    SimpleServer_XmlSchema_NamespaceUri = "nsu=http://samplecompany.com/SimpleServer/Model;i=63"
    SimpleServer_XmlSchema_Deprecated = "nsu=http://samplecompany.com/SimpleServer/Model;i=64"
    SimpleServer_XmlSchema_CycleStepDataType = "nsu=http://samplecompany.com/SimpleServer/Model;i=65"

def get_VariableIds_name(value: str) -> str:
    try:
        return VariableIds(value).name
    except ValueError:
        return None

