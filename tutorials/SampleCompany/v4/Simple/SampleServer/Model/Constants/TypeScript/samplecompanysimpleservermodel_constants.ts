export const NS = 'http://samplecompany.com/SimpleServer/Model';

export class BrowseNames {
   static readonly CurrentStep: string = 'CurrentStep'
   static readonly CycleId: string = 'CycleId'
   static readonly CycleStepDataType: string = 'CycleStepDataType'
   static readonly Error: string = 'Error'
   static readonly SimpleServer_BinarySchema: string = 'SampleCompany.SimpleServer.Model'
   static readonly SimpleServer_XmlSchema: string = 'SampleCompany.SimpleServer.Model'
   static readonly Steps: string = 'Steps'
   static readonly SystemCycleAbortedEventType: string = 'SystemCycleAbortedEventType'
   static readonly SystemCycleFinishedEventType: string = 'SystemCycleFinishedEventType'
   static readonly SystemCycleStartedEventType: string = 'SystemCycleStartedEventType'
   static readonly SystemCycleStatusEventType: string = 'SystemCycleStatusEventType'
}

export class DataTypeIds {
    static readonly CycleStepDataType: string = 'nsu=' + NS + ';i=1'
}

export class ObjectIds {
    static readonly CycleStepDataType_Encoding_DefaultBinary: string = 'nsu=' + NS + ';i=52'
    static readonly CycleStepDataType_Encoding_DefaultXml: string = 'nsu=' + NS + ';i=60'
    static readonly CycleStepDataType_Encoding_DefaultJson: string = 'nsu=' + NS + ';i=68'
}

export class ObjectTypeIds {
    static readonly SystemCycleStatusEventType: string = 'nsu=' + NS + ';i=2'
    static readonly SystemCycleStartedEventType: string = 'nsu=' + NS + ';i=14'
    static readonly SystemCycleAbortedEventType: string = 'nsu=' + NS + ';i=27'
    static readonly SystemCycleFinishedEventType: string = 'nsu=' + NS + ';i=40'
}

export class VariableIds {
    static readonly SystemCycleStatusEventType_CycleId: string = 'nsu=' + NS + ';i=12'
    static readonly SystemCycleStatusEventType_CurrentStep: string = 'nsu=' + NS + ';i=13'
    static readonly SystemCycleStartedEventType_Steps: string = 'nsu=' + NS + ';i=26'
    static readonly SystemCycleAbortedEventType_Error: string = 'nsu=' + NS + ';i=39'
    static readonly SimpleServer_BinarySchema: string = 'nsu=' + NS + ';i=53'
    static readonly SimpleServer_BinarySchema_NamespaceUri: string = 'nsu=' + NS + ';i=55'
    static readonly SimpleServer_BinarySchema_Deprecated: string = 'nsu=' + NS + ';i=56'
    static readonly SimpleServer_BinarySchema_CycleStepDataType: string = 'nsu=' + NS + ';i=57'
    static readonly SimpleServer_XmlSchema: string = 'nsu=' + NS + ';i=61'
    static readonly SimpleServer_XmlSchema_NamespaceUri: string = 'nsu=' + NS + ';i=63'
    static readonly SimpleServer_XmlSchema_Deprecated: string = 'nsu=' + NS + ';i=64'
    static readonly SimpleServer_XmlSchema_CycleStepDataType: string = 'nsu=' + NS + ';i=65'
}
