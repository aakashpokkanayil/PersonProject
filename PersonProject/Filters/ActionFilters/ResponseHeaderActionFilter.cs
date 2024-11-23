using Microsoft.AspNetCore.Mvc.Filters;

namespace PersonProject.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        private readonly string _key;
        private readonly string _value;

        // see like previous code we cant inject logger here coz FilterAttribute dont supp ctor injection.
        public ResponseHeaderActionFilter(string key,
            string value,int order)
        {
            _key = key;
            _value = value;
            Order = order;
        }


        public override async Task  OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // logic for OnActionExecuting
            context.HttpContext.Response.Headers[_key] = _value;

            await next();// this will invoke next filter in the filter pipeline
            // logic for OnActionExecuted
           
            
        }
    }
}
