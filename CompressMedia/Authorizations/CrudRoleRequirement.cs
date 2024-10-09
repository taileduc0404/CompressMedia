using Microsoft.AspNetCore.Authorization;

namespace CompressMedia.Authorizations
{
    public class CrudRoleRequirement : AuthorizationHandler<CrudRoleRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CrudRoleRequirement requirement)
        {


            throw new NotImplementedException();
        }
    }
}
