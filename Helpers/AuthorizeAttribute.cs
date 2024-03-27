using CRUDinNETCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDinNETCORE.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User?)context.HttpContext.Items["User"];
            if(user == null) 
            {
                context.Result = new JsonResult(new { message  = "UnAuthorizeed" }) { StatusCode = StatusCodes.Status401Unauthorized};
            }
        }
    }
}
