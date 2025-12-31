namespace SampleCompany.SimpleServer.Model.WebApi
{
    /// <summary>
    /// The namespaces used in the model.
    /// </summary>
    public static class Namespaces
    {
        /// <remarks />
        public const string Uri = "http://samplecompany.com/SimpleServer/Model";
    }

    /// <summary>
    /// The browse names defined in the model.
    /// </summary>
    public static class BrowseNames
    {
        /// <remarks />
        public const string CurrentStep = "CurrentStep";
        /// <remarks />
        public const string CycleId = "CycleId";
        /// <remarks />
        public const string CycleStepDataType = "CycleStepDataType";
        /// <remarks />
        public const string Error = "Error";
        /// <remarks />
        public const string SimpleServer_BinarySchema = "SampleCompany.SimpleServer.Model";
        /// <remarks />
        public const string SimpleServer_XmlSchema = "SampleCompany.SimpleServer.Model";
        /// <remarks />
        public const string Steps = "Steps";
        /// <remarks />
        public const string SystemCycleAbortedEventType = "SystemCycleAbortedEventType";
        /// <remarks />
        public const string SystemCycleFinishedEventType = "SystemCycleFinishedEventType";
        /// <remarks />
        public const string SystemCycleStartedEventType = "SystemCycleStartedEventType";
        /// <remarks />
        public const string SystemCycleStatusEventType = "SystemCycleStatusEventType";
    }

    /// <summary>
    /// The well known identifiers for DataType nodes.
    /// </summary>
    public static class DataTypeIds {
        /// <remarks />
        public const string CycleStepDataType = "nsu=" + Namespaces.Uri + ";i=1";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(DataTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Object nodes.
    /// </summary>
    public static class ObjectIds {
        /// <remarks />
        public const string CycleStepDataType_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=52";
        /// <remarks />
        public const string CycleStepDataType_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=60";
        /// <remarks />
        public const string CycleStepDataType_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=68";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for ObjectType nodes.
    /// </summary>
    public static class ObjectTypeIds {
        /// <remarks />
        public const string SystemCycleStatusEventType = "nsu=" + Namespaces.Uri + ";i=2";
        /// <remarks />
        public const string SystemCycleStartedEventType = "nsu=" + Namespaces.Uri + ";i=14";
        /// <remarks />
        public const string SystemCycleAbortedEventType = "nsu=" + Namespaces.Uri + ";i=27";
        /// <remarks />
        public const string SystemCycleFinishedEventType = "nsu=" + Namespaces.Uri + ";i=40";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Variable nodes.
    /// </summary>
    public static class VariableIds {
        /// <remarks />
        public const string SystemCycleStatusEventType_CycleId = "nsu=" + Namespaces.Uri + ";i=12";
        /// <remarks />
        public const string SystemCycleStatusEventType_CurrentStep = "nsu=" + Namespaces.Uri + ";i=13";
        /// <remarks />
        public const string SystemCycleStartedEventType_Steps = "nsu=" + Namespaces.Uri + ";i=26";
        /// <remarks />
        public const string SystemCycleAbortedEventType_Error = "nsu=" + Namespaces.Uri + ";i=39";
        /// <remarks />
        public const string SimpleServer_BinarySchema = "nsu=" + Namespaces.Uri + ";i=53";
        /// <remarks />
        public const string SimpleServer_BinarySchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=55";
        /// <remarks />
        public const string SimpleServer_BinarySchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=56";
        /// <remarks />
        public const string SimpleServer_BinarySchema_CycleStepDataType = "nsu=" + Namespaces.Uri + ";i=57";
        /// <remarks />
        public const string SimpleServer_XmlSchema = "nsu=" + Namespaces.Uri + ";i=61";
        /// <remarks />
        public const string SimpleServer_XmlSchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=63";
        /// <remarks />
        public const string SimpleServer_XmlSchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=64";
        /// <remarks />
        public const string SimpleServer_XmlSchema_CycleStepDataType = "nsu=" + Namespaces.Uri + ";i=65";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(VariableIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }
    
}