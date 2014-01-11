using System.Runtime.Serialization;

namespace Valley.Models
{
    [DataContract(Namespace = "urn:valley")]
    public class PropertyValue : IPropertyValue
    {
        [DataMember]
        public bool IsDirty { get; set; }

        [DataMember]
        public IProperty Property { get; set; }

        [DataMember]
        public object Value { get; set; }

        [DataMember]
        public bool UsingDefaultValue { get; set; }
    }
}
