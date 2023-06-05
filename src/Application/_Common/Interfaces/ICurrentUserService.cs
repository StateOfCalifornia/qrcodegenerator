using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string OnPremSid { get; }
        bool IsAuthenticated { get; }
        string Email { get; }
        string IpAddress { get; }
        string IdentityName { get; }
        string FullName { get; }
        string FirstName { get; }
        string LastName { get; }
        string UserAgent { get; }
        IEnumerable<string> Roles { get; }
        bool UserIsInRole(string role);
    }
}