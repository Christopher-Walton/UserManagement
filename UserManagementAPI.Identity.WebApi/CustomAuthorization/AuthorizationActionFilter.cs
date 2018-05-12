using Ninject.Web.WebApi.Filter;
using System;
using System.Collections.Generic;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;
using System.Security.Principal;
using UserManagementAPI.BusinessLogic;

namespace UserManagementAPI.Identity.WebAPI.CustomAuthorization
{
    public class AuthorizationActionFilter : AbstractActionFilter
    {
        public class AuthorizationResult
        {
            public bool Authorized { get; set; }
            public List<string> Errors { get; set; }
        }

        private AuthorizationResult VerifyAuthorizationToken(UserAuthorizationInfo authInfo, string AuthorizationRoles)
        {
            if (authInfo == null)
                return new AuthorizationResult { Authorized = false };

            if (DateTime.Now > authInfo.ExpiryDate)
                return new AuthorizationResult { Authorized = false, Errors = new List<String>() { "Token expired" } };

            if (!string.IsNullOrEmpty(AuthorizationRoles) && !authInfo.Roles.Contains(AuthorizationRoles))
                return new AuthorizationResult { Authorized = false, Errors = new List<string>() { "User does not have permission to perform this action" } };

            return new AuthorizationResult { Authorized = true };
        }

        private readonly IAuthorizationManager _authorizationManagement;

        public AuthorizationActionFilter(IAuthorizationManager authorizationManagement)
        {
            _authorizationManagement = authorizationManagement;
        }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var actionAttributes = actionContext.ActionDescriptor.GetCustomAttributes<AuthorizationActionAttribute>();

            var roles = actionAttributes.First().Roles;

            var authorizationHeader = actionContext.Request.Headers.Authorization;

            if (authorizationHeader == null || string.IsNullOrEmpty(authorizationHeader.Parameter) || string.IsNullOrEmpty(authorizationHeader.Scheme))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

                //may need to include authenticate header here
            }
            else
            {
                var token = authorizationHeader.Parameter;

                var userAuthorizationInformation = _authorizationManagement.GetAuthorizationInformation(token);

                var authResult = VerifyAuthorizationToken(userAuthorizationInformation, roles);

                if (authResult.Authorized)
                {
                    
                    CustomUserPrincipal principal = new CustomUserPrincipal(new GenericIdentity(userAuthorizationInformation.UserName),
                                                                userAuthorizationInformation.Roles.ToArray(),userAuthorizationInformation.UserId);
                                  
                    actionContext.RequestContext.Principal = principal;
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    actionContext.Response.Content = new ObjectContent<List<string>>(authResult.Errors, new JsonMediaTypeFormatter());
                }
            }
        }

        public override bool AllowMultiple
        {
            get { return true; }
        }
    }
}