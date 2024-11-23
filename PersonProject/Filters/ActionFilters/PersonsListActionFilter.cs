using Microsoft.AspNetCore.Mvc.Filters;
using PersonProject.Controllers;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;

namespace PersonProject.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");

            PersonController personController= (PersonController)context.Controller;
            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["Arguments"];
            if (parameters != null)
            {
                if (parameters.ContainsKey("searchBy"))
                {
                    personController.ViewData["CurrentSearchBy"] =Convert.ToString(parameters["searchBy"]);
                }
                if (parameters.ContainsKey("searchString"))
                {
                    personController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
                }
                if (parameters.ContainsKey("sortBy"))
                {
                    personController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
                }
                else
                {
                    personController.ViewData["CurrentSortBy"] = nameof(PersonResponseDto.PersonName);
                }
                if (parameters.ContainsKey("sortOrder"))
                {
                    personController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["sortOrder"]);
                }
                else
                {
                    personController.ViewData["CurrentSortOrder"] = SortOderOption.ASC.ToString();
                }
                personController.ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { nameof(PersonResponseDto.PersonName),"Person Name"},
                    { nameof(PersonResponseDto.Email),"Email Address"},
                    { nameof(PersonResponseDto.Dob),"Date Of Birth"},
                    { nameof(PersonResponseDto.Gender),"Gender"},
                    { nameof(PersonResponseDto.Address),"Address"},
                    { nameof(PersonResponseDto.Country),"Country"}
                };
                personController.ViewBag.HeaderDict = new Dictionary<string, string>()
                {
                    {"Person Name",nameof(PersonResponseDto.PersonName) },
                    {"Email Address",nameof(PersonResponseDto.Email)},
                    {"Date of Birth",nameof(PersonResponseDto.Dob)},
                    {"Age",nameof(PersonResponseDto.Age)},
                    {"Gender",nameof(PersonResponseDto.Gender)},
                    {"Country",nameof(PersonResponseDto.Country)},
                    {"Address",nameof(PersonResponseDto.Address)},
                    {"Receive News Letters",nameof(PersonResponseDto.ReceiveNewsLetters)},
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // by doing this we can access argument in OnActionExecuted
            // coz context.ActionArguments is not avaliable in OnActionExecuted
            // context.HttpContext is avaliable everywhere controller, filters etc
            context.HttpContext.Items["Arguments"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if (!string.IsNullOrEmpty(searchBy))
                {
                    List<string> searchOptions = new List<string>()
                    {
                        nameof(PersonResponseDto.PersonName),
                        nameof(PersonResponseDto.Email),
                        nameof(PersonResponseDto.Dob),
                        nameof(PersonResponseDto.Gender),
                        nameof(PersonResponseDto.Address),
                        nameof(PersonResponseDto.Country)
                    };
                    if (searchOptions.Any(x => x == searchBy))
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                    }
                    else
                    {
                        context.ActionArguments["searchBy"] = nameof(PersonResponseDto.PersonName);
                        _logger.LogInformation("searchBy updated value {searchBy}", nameof(PersonResponseDto.PersonName));
                    }
                }
            }
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");
        }
    }
    
}
