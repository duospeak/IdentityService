using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Microsoft.AspNetCore.Authorization
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires the specified authorization.
    /// </summary>
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        /// <inheritdoc />
        public ApiAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
