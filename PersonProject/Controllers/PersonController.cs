using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;

namespace PersonProject.Controllers
{
    [Route("persons")]
    public class PersonController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonController(IPersonsService personsService,ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponseDto.PersonName), SortOderOption sortOrder = SortOderOption.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponseDto.PersonName),"Person Name"},
                { nameof(PersonResponseDto.Email),"Email Address"},
                { nameof(PersonResponseDto.Dob),"Date Of Birth"},
                { nameof(PersonResponseDto.Gender),"Gender"},
                { nameof(PersonResponseDto.Address),"Address"},
                { nameof(PersonResponseDto.Country),"Country"}
            };
            ViewBag.HeaderDict = new Dictionary<string, string>()
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
            List<PersonResponseDto>? personResponse = await _personsService.GetPersonByFilter(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            personResponse = _personsService.GetSortedPersons(personResponse, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(personResponse);
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           ViewBag.Countries= await _countriesService.GetAllCountries();
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequestDto personAddRequest)
        {
            if (!ModelState.IsValid) {
                ViewBag.Countries = await _countriesService.GetAllCountries();
                ViewBag.Errors=ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage).ToList();
                return View();
            }
            PersonResponseDto? personResponse= await _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");

        }
    }
}
