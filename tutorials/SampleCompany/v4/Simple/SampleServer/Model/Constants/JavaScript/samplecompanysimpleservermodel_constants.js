export const NS = 'http://samplecompany.com/SimpleServer/Model';

export const BrowseNames = Object.freeze({
   CurrentStep: 'CurrentStep',
   CycleId: 'CycleId',
   CycleStepDataType: 'CycleStepDataType',
   Error: 'Error',
   SimpleServer_BinarySchema: 'SampleCompany.SimpleServer.Model',
   SimpleServer_XmlSchema: 'SampleCompany.SimpleServer.Model',
   Steps: 'Steps',
   SystemCycleAbortedEventType: 'SystemCycleAbortedEventType',
   SystemCycleFinishedEventType: 'SystemCycleFinishedEventType',
   SystemCycleStartedEventType: 'SystemCycleStartedEventType',
   SystemCycleStatusEventType: 'SystemCycleStatusEventType',
});

export const DataTypeIds = Object.freeze({
   CycleStepDataType: 'nsu=' + NS + ';i=1',
});

export const ObjectIds = Object.freeze({
   CycleStepDataType_Encoding_DefaultBinary: 'nsu=' + NS + ';i=52',
   CycleStepDataType_Encoding_DefaultXml: 'nsu=' + NS + ';i=60',
   CycleStepDataType_Encoding_DefaultJson: 'nsu=' + NS + ';i=68',
});

export const ObjectTypeIds = Object.freeze({
   SystemCycleStatusEventType: 'nsu=' + NS + ';i=2',
   SystemCycleStartedEventType: 'nsu=' + NS + ';i=14',
   SystemCycleAbortedEventType: 'nsu=' + NS + ';i=27',
   SystemCycleFinishedEventType: 'nsu=' + NS + ';i=40',
});

export const VariableIds = Object.freeze({
   SystemCycleStatusEventType_CycleId: 'nsu=' + NS + ';i=12',
   SystemCycleStatusEventType_CurrentStep: 'nsu=' + NS + ';i=13',
   SystemCycleStartedEventType_Steps: 'nsu=' + NS + ';i=26',
   SystemCycleAbortedEventType_Error: 'nsu=' + NS + ';i=39',
   SimpleServer_BinarySchema: 'nsu=' + NS + ';i=53',
   SimpleServer_BinarySchema_NamespaceUri: 'nsu=' + NS + ';i=55',
   SimpleServer_BinarySchema_Deprecated: 'nsu=' + NS + ';i=56',
   SimpleServer_BinarySchema_CycleStepDataType: 'nsu=' + NS + ';i=57',
   SimpleServer_XmlSchema: 'nsu=' + NS + ';i=61',
   SimpleServer_XmlSchema_NamespaceUri: 'nsu=' + NS + ';i=63',
   SimpleServer_XmlSchema_Deprecated: 'nsu=' + NS + ';i=64',
   SimpleServer_XmlSchema_CycleStepDataType: 'nsu=' + NS + ';i=65',
});
