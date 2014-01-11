
namespace Valley.Models
{
    public interface IPropertyValue
    {
        bool IsDirty { get; set; }
        IProperty Property { get; set; }
        object Value { get; set; }
        bool UsingDefaultValue { get; set; }
    }
}
