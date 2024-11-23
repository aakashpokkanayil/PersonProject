using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonProject.Filters.AuthFilter
{
    public class TokenAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // after server generates token and send to client.
            // for each request brower will send token as cookie.
            // which can be accessed like below.
            if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Token"))
            {
                // this block will work is token cookie is not present.
                // so we are short circuiting here
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;// immediatly exit this filter after short circuiting.
            }

            if (context.HttpContext.Request.Cookies["Auth-Token"] != "001A")
            {
                // this block will work is token cookie is present but not matches with server token.
                // this is only a sample code.
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;// immediatly exit this filter after short circuiting.
            }
        }
    }
}
