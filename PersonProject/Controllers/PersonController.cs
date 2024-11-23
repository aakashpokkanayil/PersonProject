using Microsoft.AspNetCore.Mvc;
using PersonProject.Filters.ActionFilters;
using PersonProject.Filters.AuthFilter;
using PersonProject.Filters.ResultFilters;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;

namespace PersonProject.Controllers
{
    [Route("persons")]
    //[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-CustomController-Key", "Custom-Value" ,3},Order =3)]
    [ResponseHeaderActionFilter("X-CustomController-Key", "Custom-Value", 3)]
    public class PersonController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonsService personsService,ICountriesService countriesService,ILogger<PersonController> logger)
        {
            _personsService = personsService;
            _countriesService = countriesService;
            _logger = logger;
        }

        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        //[TypeFilter(typeof(ResponseHeaderActionFilter),Arguments =new object[] { "X-CustomAction-Key","Custom-Value",2}
        //,Order =2)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [ResponseHeaderActionFilter("X-CustomAction-Key", "Custom-Value", 2)]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponseDto.PersonName), SortOderOption sortOrder = SortOderOption.ASC)
        {
            _logger.LogInformation("PersonController:Index Action");
            _logger.LogDebug($"searchBy:{searchBy}, searchString:{searchString}, sortBy:{sortBy}, sortOrder:{sortOrder}");

            
            List<PersonResponseDto>? personResponse = await _personsService.GetPersonByFilter(searchBy, searchString);
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchString = searchString;

            personResponse = _personsService.GetSortedPersons(personResponse, sortBy, sortOrder);
            //ViewBag.CurrentSortBy = sortBy;
            //ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(personResponse);
        }


        [Route("[action]")]
        [HttpGet]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Create()
        {
           ViewBag.Countries= await _countriesService.GetAllCountries();
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(ModelStateValidationActionFilter))]
        [TypeFilter(typeof(TokenAuthFilter))]
        public async Task<IActionResult> Create(PersonAddRequestDto personAddRequest)
        {
            
            PersonResponseDto? personResponse= await _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");

        }
    }
}
