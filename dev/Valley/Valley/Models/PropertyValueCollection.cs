using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Valley.Models
{
    [DataContract(Namespace = "urn:valley")]
    public class PropertyValueCollection : List<IPropertyValue>, IPropertyValueCollection
    {
    }
}
