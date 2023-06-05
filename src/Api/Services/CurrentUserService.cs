namespace Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    #region Constructor
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    #region ICurrentUserService Implementation
    /// <summary>Object ID</summary>
    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
    /// <summary>Security Identifier (Sid)</summary>
    public string OnPremSid => _httpContextAccessor.HttpContext?.User?.FindFirstValue("onprem_sid") ?? null;
    /// <summary>Flag to tell whether the user is logged on or not. If the UserId is populated, then this will be true</summary>
    public bool IsAuthenticated => UserId != null;
    /// <summary>Email Address</summary>
    public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)?.ToLower() ?? null;
    /// <summary>Inbound client IP address</summary>
    public string IpAddress => IsAuthenticated ? GetIpAddress() : _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
    /// <summary>Concatination of First and Last names</summary>
    public string FullName => $"{FirstName} {LastName}";
    /// <summary>Identity name</summary>
    public string IdentityName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("name");
    /// <summary>First Name</summary>
    public string FirstName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);
    /// <summary>Last Name</summary>
    public string LastName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname);
    /// <summary>Inbound Users Agent</summary>
    public string UserAgent => _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.UserAgent].ToString();
    /// <summary>Roles</summary>
    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

    /// <summary>Checks to see whether or not the user is in the inbound role</summary>
    /// <param name="role">role to check</param>
    /// <returns>True/False whether the user is in the role</returns>
    public bool UserIsInRole(string role)
    {
        return _httpContextAccessor.HttpContext.User.IsInRole(role);
    }
    #endregion

    #region Private Helpers
    /// <summary>
    /// Gets the IP address and masks the last octet in order to anonymize the exact ip
    /// </summary>
    /// <returns>Anonymized user IP address</returns>
    private string GetIpAddress()
    {
        var ip = _httpContextAccessor.HttpContext?.User?.FindFirstValue("ipaddr");

        //If the IP is there, we need to anonymize (mask) the last octet and set it to '0' (zro)
        if (!string.IsNullOrEmpty(ip))
        {
            var ipArray = ip.Split(".").ToArray();
            return $"{ipArray[0]}.{ipArray[1]}.{ipArray[2]}.0";
        }
        return ip;
    }
    #endregion
}
