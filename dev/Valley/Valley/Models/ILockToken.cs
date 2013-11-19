using System;

namespace Valley.Models
{
    public interface ILockToken : IResource
    {
        bool IsExpired();
        Uri Resource { get; set; }
        // RFC.4918 §7.7
        TimeSpan Timeout { get; set; }
        // RFC.4918 §6.5
        string Value { get; }
        Uri LockUri { get; }
    }
}
