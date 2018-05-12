using System.Security.Principal;

namespace UserManagement.Identity.UI.CustomAuthorization
{
    public class CustomUserPrincipal : GenericPrincipal
    {
        public CustomUserPrincipal(IIdentity identity, string[] roles, string userName, string authorizationToken, string userId)
            : base(identity, roles)
        {
            this.UserName = userName;
            this.AuthorizationToken = authorizationToken;
            this.UserId = userId;
        }

        private readonly string UserName;
        private readonly string AuthorizationToken;
        private readonly string UserId;

        public string GetUserName()
        {
            return UserName;
        }

        public string GetAuthorizationToken()
        {
            return AuthorizationToken;
        }

        public string GetUserId()
        {
            return UserId;
        }
    }
}