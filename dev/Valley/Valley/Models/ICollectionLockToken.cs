
namespace Valley.Models
{
    // RFC.4918 §8.3: Identifiers for collections SHOULD end in a '/' character.
    public interface ICollectionLockToken : ILockToken
    {
        CollectionLockTokenDepth Depth { get; }
    }
}
