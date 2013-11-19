using System;
using System.Runtime.Serialization;

namespace Valley.Models
{
    [DataContract(Namespace = "urn:valley")]
    public class Property : IProperty
    {
        [DataMember]
        public object DefaultValue { get; set; }

        [DataMember]
        public bool IsReadOnly { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Type PropertyType { get; set; }

        [DataMember]
        public string BaseType { get; set; }
    }
}
