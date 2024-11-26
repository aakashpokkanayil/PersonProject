using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonProject.Controllers;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;

namespace ProjectTest.ControllerTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonController> _logger;

        private readonly Mock<IPersonsAdderService> _mockPersonsAdderService;
        private readonly Mock<IPersonsGetterService> _mockPersonsGetterService;
        private readonly Mock<IPersonsSorterService> _mockPersonsSorterService;
        private readonly Mock<ICountriesService> _mockCountriesService;
        private readonly Mock<ILogger<PersonController>> _mockLogger;
        public PersonsControllerTest()
        {
            _mockCountriesService = new Mock<ICountriesService>();
            _mockPersonsAdderService = new Mock<IPersonsAdderService>();
            _mockPersonsGetterService = new Mock<IPersonsGetterService>();
            _mockPersonsSorterService = new Mock<IPersonsSorterService>();
            _mockLogger = new Mock<ILogger<PersonController>>();

            _personsAdderService = _mockPersonsAdderService.Object;
            _personsGetterService = _mockPersonsGetterService.Object; 
            _personsSorterService = _mockPersonsSorterService.Object;
            _countriesService = _mockCountriesService.Object;
            _logger = _mockLogger.Object;
        }

        #region Index
        [Fact]
        public async Task Index_WithProperdetails()
        {
            //Arrange
            List<PersonResponseDto> personResponseDtos = new List<PersonResponseDto>()
            {
                new PersonResponseDto()
                {
                    PersonId = Guid.NewGuid(),
                    PersonName = "Kokash",
                    Email = "kokash@mail.com",
                    Address = "kokash Address",
                    CountryId = Guid.NewGuid(),
                    Gender = "Male",
                    Dob = DateTime.Parse("1996-5-13"),
                    ReceiveNewsLetters = true
                },
                new PersonResponseDto()
                {
                    PersonId = Guid.NewGuid(),
                    PersonName = "Aakash",
                    Email = "Aakash@mail.com",
                    Address = "Aakash Address",
                    CountryId = Guid.NewGuid(),
                    Gender = "Male",
                    Dob = DateTime.Parse("1996-5-13"),
                    ReceiveNewsLetters = true
                }
            };

            PersonController personController = new PersonController(_countriesService, _logger,_personsAdderService, _personsGetterService,_personsSorterService);

            _mockPersonsGetterService.Setup(x => x.GetPersonByFilter(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personResponseDtos);
            _mockPersonsSorterService.Setup(x => x.GetSortedPersons(It.IsAny<List<PersonResponseDto>>(), It.IsAny<string>(), It.IsAny<SortOderOption>()))
                .Returns(personResponseDtos);
            //Act
            IActionResult actionResult = await personController.Index("PersonName", "PersonName", SortOderOption.ASC.ToString());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.Equal(viewResult.Model, personResponseDtos);
        }
        #endregion

        #region Create
        [Fact]
        public async Task Create_WithValidDetails()
        {
            //Arrange
            Guid country1Guid = Guid.NewGuid();
            Guid country2Guid = Guid.NewGuid();
            List<CountryResponseDto> countries = new List<CountryResponseDto>()
            {
                new CountryResponseDto() {CountryId=country1Guid, CountryName = "Canada"},
                new CountryResponseDto() {CountryId=country2Guid, CountryName = "India" }
            };
            PersonAddRequestDto personRequestDtos = new PersonAddRequestDto()
            {
                PersonName = "Kokash",
                Email = "kokash@mail.com",
                Address = "kokash Address",
                CountryId = country1Guid,
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };

            PersonResponseDto personResponseDto = new PersonResponseDto()
            {
                PersonId = Guid.NewGuid(),
                PersonName = "Kokash",
                Email = "kokash@mail.com",
                Address = "kokash Address",
                CountryId = country1Guid,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
           
            _mockCountriesService.Setup(x=>x.GetAllCountries()).ReturnsAsync(countries);
            _mockPersonsAdderService.Setup(x=>x.AddPerson(It.IsAny<PersonAddRequestDto>())).ReturnsAsync(personResponseDto);

            PersonController personController = new PersonController(_countriesService, _logger, _personsAdderService, _personsGetterService, _personsSorterService);

           
            //Act
            IActionResult actionResult = await personController.Create(personRequestDtos);

            //Assert
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(actionResult);
            Assert.Equal(redirectToActionResult.ActionName, "Index");
        }
        #endregion
    }
}
