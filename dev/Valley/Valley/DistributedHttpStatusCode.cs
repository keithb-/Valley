
namespace Valley
{
    public class DistributedHttpStatusCode : Enumeration
    {
        private DistributedHttpStatusCode() { }
        private DistributedHttpStatusCode(int value) : base(value) { }
        public static readonly DistributedHttpStatusCode Locked = new DistributedHttpStatusCode(423);
    }
}