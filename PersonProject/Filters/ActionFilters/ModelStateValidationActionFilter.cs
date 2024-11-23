using Microsoft.AspNetCore.Mvc.Filters;
using PersonProject.Controllers;
using ServiceContracts.Interfaces.Country;

namespace PersonProject.Filters.ActionFilters
{
    public class ModelStateValidationActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;

        public ModelStateValidationActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // OnActionExecuting part
            if (context.Controller is PersonController personController)
            {
                if (!personController.ModelState.IsValid)
                {
                    personController.ViewBag.Countries = await _countriesService.GetAllCountries();
                    personController.ViewBag.Errors = personController.ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage).ToList();
                    // by setting view to context.Result we are short Circuting.
                    // means now action wont work if modelstate is invalid. Instead context.Result will work.
                    context.Result = personController.View();
                }
                else
                {
                    await next(); // for avoid short Circuting if model state if valid
                }
            }
            else
            {
                await next(); // for avoid short Circuting if Controller is not PersonController
            }
            
            
            // OnActionExecuted part
        }
    }
}
