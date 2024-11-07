using AutoMapper;
using Entities.CountryEntity;
using Entities.PersonEntity;
using Moq;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;
using Services.CountryService;
using Services.PersonService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.PersonUnitTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService? _personsService;
        private readonly Mock<IMapper>? _mockMapper;
        private readonly ICountriesService? _countriesService;
        private PersonAddRequestDto? _personAddRequestDto = null;
        private PersonAddRequestDto? _personAddRequestDto2 = null;
        private PersonAddRequestDto? _personAddRequestDto3 = null;
        private CountryAddRequestDto? _countryAddRequestDto = null;
        private CountryAddRequestDto? _countryAddRequestDto2 = null;
        private CountryAddRequestDto? _countryAddRequestDto3 = null;
        private PersonUpdateRequestDto? _personUpdateRequestDto = null;

        public PersonsServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _countriesService = new CountriesService(_mockMapper.Object);
            _personsService = new PersonsServices(_mockMapper.Object, _countriesService);

            Initializeobjects();
            mockPersonMapper();
            mockCountryMapper();
        }
        #region PrivateMethods
        private void mockPersonMapper()
        {
            _mockMapper.Setup(m => m.Map<Person>(It.IsAny<PersonAddRequestDto>()))
                .Returns<PersonAddRequestDto>((req) => new Person
                {
                    PersonName = req.PersonName,
                    Email = req.Email,
                    Address = req.Address,
                    CountryId = req.CountryId,
                    Gender = (Entities.Enum.Gender?)req.Gender,
                    Dob = req.Dob,
                    ReceiveNewsLetters = req.ReceiveNewsLetters
                });
            _mockMapper.Setup(m => m.Map<PersonResponseDto>(It.IsAny<Person>()))
                .Returns<Person>((p) => new PersonResponseDto
                {
                    PersonId = p.PersonId,
                    PersonName = p.PersonName,
                    Email = p.Email,
                    Dob = p.Dob,
                    Address = p.Address,
                    CountryId = p.CountryId,
                    Gender = (Gender?)p.Gender,
                    ReceiveNewsLetters = p.ReceiveNewsLetters
                });
            _mockMapper.Setup(m => m.Map<List<PersonResponseDto>>(It.IsAny<List<Person>>()))
                .Returns<List<Person>>((pList) => pList.Select((pRes) => new PersonResponseDto
                {
                    PersonId = pRes.PersonId,
                    PersonName = pRes.PersonName,
                    Email = pRes.Email,
                    Dob = pRes.Dob,
                    Address = pRes.Address,
                    CountryId = pRes.CountryId,
                    Gender = (Gender?)pRes.Gender,
                    ReceiveNewsLetters = pRes.ReceiveNewsLetters
                }).ToList());

        }

        private void mockCountryMapper()
        {
            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns<CountryAddRequestDto>((cReq) => new Country()
                {
                    CountryName = cReq.CountryName,
                });
            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
               .Returns<Country>((c) => new CountryResponseDto()
               {
                   CountryId = c.CountryId,
                   CountryName = c.CountryName
               });
        }

        private void AddPersonDetails()
        {
            CountryResponseDto country1 = _countriesService.AddCountry(_countryAddRequestDto);
            CountryResponseDto country2 = _countriesService.AddCountry(_countryAddRequestDto2);
            CountryResponseDto country3 = _countriesService.AddCountry(_countryAddRequestDto3);
            _personAddRequestDto.CountryId = country1.CountryId;
            _personAddRequestDto2.CountryId = country2.CountryId;
            _personAddRequestDto3.CountryId = country3.CountryId;

            _personsService.AddPerson(_personAddRequestDto);
            _personsService.AddPerson(_personAddRequestDto2);
            _personsService.AddPerson(_personAddRequestDto3);
        }

        private void Initializeobjects()
        {
            _countryAddRequestDto = new CountryAddRequestDto() { CountryName = "Canada" };
            _countryAddRequestDto2 = new CountryAddRequestDto() { CountryName = "India" };
            _countryAddRequestDto3 = new CountryAddRequestDto() { CountryName = "America" };
            _personAddRequestDto = new PersonAddRequestDto()
            {
                PersonName = "Aakash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = Guid.NewGuid(),
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            _personAddRequestDto2 = new PersonAddRequestDto()
            {
                PersonName = "Deva",
                Email = "Deva@mail.com",
                Address = "Deva Address",
                CountryId = Guid.NewGuid(),
                Gender = Gender.Male,
                Dob = DateTime.Parse("1999-7-16"),
                ReceiveNewsLetters = true
            };
            _personAddRequestDto3 = new PersonAddRequestDto()
            {
                PersonName = "Abhi",
                Email = "Abhi@mail.com",
                Address = "Abhi Address",
                CountryId = Guid.NewGuid(),
                Gender = Gender.Male,
                Dob = DateTime.Parse("1997-3-26"),
                ReceiveNewsLetters = true
            };
            _personUpdateRequestDto = new PersonUpdateRequestDto()
            {
                PersonId = Guid.Empty,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = Guid.NewGuid(),
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
        }
        #endregion

        #region AddPerson

        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequestDto? personAddRequestDto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequestDto);
            });


        }

        [Fact]
        public void AddPerson_PersonNameNull()
        {
            //Arrange
            PersonAddRequestDto personAddRequestDto = new PersonAddRequestDto()
            {
                PersonName = null,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personsService.AddPerson(personAddRequestDto);
            });
        }



        [Fact]
        public void AddPerson_WithProperDetails()
        {
            //Arrange


            //Act

            PersonResponseDto? actualPersonResponse = _personsService.AddPerson(_personAddRequestDto);

            List<PersonResponseDto>? expectedPersonResponseList = _personsService.GetAllPerson();

            //Assert
            Assert.True(actualPersonResponse?.PersonId != Guid.Empty);
            Assert.Contains(actualPersonResponse, expectedPersonResponseList);

        }
        #endregion

        #region GetAllPerson
        [Fact]
        public void GetAllPerson_NullList()
        {
            //Arrange

            //Act
            List<PersonResponseDto>? personResponseDto = _personsService.GetAllPerson();
            //Assert
            Assert.Empty(personResponseDto);
        }
        [Fact]
        public void GetAllPerson_WithProperdetails()
        {
            //Arrange
            CountryResponseDto countryResponseDto = _countriesService.AddCountry(_countryAddRequestDto);
            _personAddRequestDto.CountryId = countryResponseDto.CountryId;
            PersonResponseDto? personResponse = _personsService.AddPerson(_personAddRequestDto);
            //Act
            List<PersonResponseDto>? personResponseList = _personsService.GetAllPerson();
            //Assert
            Assert.NotNull(personResponseList);
            Assert.Contains(personResponse, personResponseList);
            Assert.True(personResponseList.Exists(x => x.CountryId == countryResponseDto.CountryId));
        }
        #endregion

        #region GetPersonById
        public void GetPersonById_NullPersonId()
        {
            // Arrange
            Guid? personID = null;
            // Act
            PersonResponseDto? personResponseDto = _personsService.GetPersonById(personID);
            // Assert
            Assert.Null(personResponseDto);

        }
        [Fact]
        public void GetPersonById_WithProperId()
        {
            // Arrange
            CountryResponseDto countryResponseDto = _countriesService.AddCountry(_countryAddRequestDto);
            PersonResponseDto? expectedPersonResponse = _personsService.AddPerson(_personAddRequestDto);


            // Act
            PersonResponseDto? actualPersonResponse = _personsService.GetPersonById(expectedPersonResponse?.PersonId);
            // Assert
            Assert.Equal(expectedPersonResponse, actualPersonResponse);

        }

        #endregion

        #region GetPersonFiltered
        [Fact]
        public void GetFilteredPerson_EmptySearchTest()
        {
            //Arrange

            AddPersonDetails();

            List<PersonResponseDto> expectedPersonResponseList = _personsService.GetAllPerson();

            //Act

            List<PersonResponseDto> actualPersonResponseList= 
                _personsService.GetPersonByFilter(nameof(Person.PersonName),"");

            // Assert

            foreach (var person in expectedPersonResponseList)
            {
                Assert.Contains(person, actualPersonResponseList);
            }




        }

        [Fact]
        public void GetFilteredPerson_SearchByPersonName()
        {
            //Arrange

            AddPersonDetails();

            List<PersonResponseDto> expectedPersonResponseList = _personsService.GetAllPerson();

            //Act

            List<PersonResponseDto> actualPersonResponseList =
                _personsService.GetPersonByFilter(nameof(Person.PersonName), "v");

            // Assert

            foreach (var person in expectedPersonResponseList)
            {
                if (person.PersonName != null)
                {
                    if (person.PersonName.Contains("V", StringComparison.OrdinalIgnoreCase))
                        Assert.Contains(person, actualPersonResponseList);
                }
            }




        }
        #endregion

        #region GetSortedPerson
        [Fact]
        public void GetSortedPerson_ByPersonName_Desc()
        {
            //Arrange

            AddPersonDetails();

            List<PersonResponseDto> PersonResponseList = _personsService.GetAllPerson();
            List<PersonResponseDto> expectedPersonResponseList= 
                PersonResponseList.OrderByDescending(person => person.PersonName).ToList();


            //Act

            List<PersonResponseDto> actualPersonResponseList =
                _personsService.GetSortedPersons(PersonResponseList, nameof(Person.PersonName), SortOderOption.DESC);

            // Assert

            for (int i = 0; i < expectedPersonResponseList.Count; i++)
            {
                Assert.Equal(expectedPersonResponseList[i], actualPersonResponseList[i]);
            }




        }

        [Fact]
        public void GetSortedPerson_ByPersonName_Asc()
        {
            //Arrange

            AddPersonDetails();

            List<PersonResponseDto> PersonResponseList = _personsService.GetAllPerson();
            List<PersonResponseDto> expectedPersonResponseList =
                PersonResponseList.OrderBy(person => person.PersonName).ToList();


            //Act

            List<PersonResponseDto> actualPersonResponseList =
                _personsService.GetSortedPersons(PersonResponseList, nameof(Person.PersonName), SortOderOption.ASC);

            // Assert

            for (int i = 0; i < expectedPersonResponseList.Count; i++)
            {
                Assert.Equal(expectedPersonResponseList[i], actualPersonResponseList[i]);
            }




        }
        #endregion

        #region UpdatePerson
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequestDto personUpdateRequestDto = null;

            
            // Assert
            Assert.Throws<ArgumentNullException>(() => {
                // Act
                _personsService.UpdatePerson(personUpdateRequestDto);
            });

        }
        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            // Arrange
            PersonUpdateRequestDto personUpdateRequestDto = new PersonUpdateRequestDto() {PersonId=Guid.NewGuid() };


            // Assert
            Assert.Throws<ArgumentException>(() => {
                // Act
                _personsService.UpdatePerson(personUpdateRequestDto);
            });

        }
        [Fact]
        public void UpdatePerson_PersonNameNull()
        {
            // Arrange
            CountryResponseDto countryResponseDto = _countriesService.AddCountry(_countryAddRequestDto);
            PersonResponseDto? personResponseToUpdate = _personsService.AddPerson(_personAddRequestDto);
            PersonUpdateRequestDto personUpdateRequestDto = new PersonUpdateRequestDto()
            {
                PersonId = personResponseToUpdate.PersonId,
                PersonName = null,
            };


            // Assert
            Assert.Throws<ArgumentException>(() => {
                // Act
                _personsService.UpdatePerson(personUpdateRequestDto);
            });

        }
        [Fact]
        public void UpdatePerson_WithProperDetails()
        {
            // Arrange
            CountryResponseDto countryResponseDto = _countriesService.AddCountry(_countryAddRequestDto);
            PersonResponseDto? personResponseToUpdate = _personsService.AddPerson(_personAddRequestDto);
            _personUpdateRequestDto.PersonId = personResponseToUpdate.PersonId;


            // Act
            PersonResponseDto? actualPersonResponse = _personsService.UpdatePerson(_personUpdateRequestDto);
            PersonResponseDto? expectedPersonResponse = _personsService.GetPersonById(actualPersonResponse?.PersonId);

            // Assert
            Assert.Equal(expectedPersonResponse, actualPersonResponse);

        }
        #endregion


        [Fact]
        public void DeletePerson_WithoutProperDetails()
        {
            //Arrange
             Guid personId= Guid.NewGuid();
            //Act
            bool isdeleted = _personsService.DeletePerson(personId);

            //Assert
            Assert.False(isdeleted);
        }

        [Fact]
        public void DeletePerson_WithProperDetails()
        {
            //Arrange
            PersonResponseDto person = _personsService.AddPerson(_personAddRequestDto);

            //Act
            bool isdeleted = _personsService.DeletePerson(person.PersonId);

            //Assert
            Assert.True(isdeleted); 
        }
    }
}
