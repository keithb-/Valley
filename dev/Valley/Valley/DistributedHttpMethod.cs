using System.Net.Http;

namespace Valley
{
    public class DistributedHttpMethod : HttpMethod
    {
        protected DistributedHttpMethod(string method) : base(method) { }
        public static readonly DistributedHttpMethod Unlock = new DistributedHttpMethod("UNLOCK");
        public static readonly DistributedHttpMethod Lock = new DistributedHttpMethod("LOCK");
        public static readonly DistributedHttpMethod Copy = new DistributedHttpMethod("COPY");
        public static readonly DistributedHttpMethod Move = new DistributedHttpMethod("MOVE");
        public static readonly DistributedHttpMethod PropertyPatch = new DistributedHttpMethod("PROPPATCH");
        public static readonly DistributedHttpMethod MakeCollection = new DistributedHttpMethod("MKCOL");
    }
}