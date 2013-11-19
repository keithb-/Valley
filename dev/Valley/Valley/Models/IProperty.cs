using System;

namespace Valley.Models
{
    public interface IProperty
    {
        object DefaultValue { get; set; }
        bool IsReadOnly { get; set; }
        string Name { get; set; }
        Type PropertyType { get; set; }
        string BaseType { get; set; }
    }
}
