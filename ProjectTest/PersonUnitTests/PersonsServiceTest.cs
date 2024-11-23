using AutoMapper;
using Entities.CountryEntity;
using Entities.PersonEntity;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Person;
using Services.PersonService;

namespace ProjectTest.PersonUnitTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService? _personsService;
        private readonly Mock<IMapper>? _mockMapper;
        private PersonAddRequestDto? _personAddRequestDto = null;
        private PersonAddRequestDto? _personAddRequestDto2 = null;
        private PersonAddRequestDto? _personAddRequestDto3 = null;
        private CountryAddRequestDto? _countryAddRequestDto = null;
        private CountryAddRequestDto? _countryAddRequestDto2 = null;
        private CountryAddRequestDto? _countryAddRequestDto3 = null;
        private PersonUpdateRequestDto? _personUpdateRequestDto = null;
        private readonly IPersonRepository _personRepository;
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly Mock<ILogger<PersonsServices>> _mockLogger;
        private readonly ILogger<PersonsServices> _logger;

        public PersonsServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockPersonRepository= new Mock<IPersonRepository>();
            _mockLogger= new Mock<ILogger<PersonsServices>>();   
            _personRepository= _mockPersonRepository.Object;
            _logger=_mockLogger.Object;

            _personsService = new PersonsServices(_mockMapper.Object, _personRepository, _logger);

            Initializeobjects();
            mockPersonMapper();
            mockCountryMapper();
        }
        #region PrivateMethods
        private void  mockPersonMapper()
        {
           _mockMapper?.Setup(m => m.Map<Person>(It.IsAny<PersonAddRequestDto>()))
                .Returns<PersonAddRequestDto>((req) => new Person
                {
                    PersonName = req.PersonName,
                    Email = req.Email,
                    Address = req.Address,
                    CountryId = req.CountryId,
                    Gender = req.Gender.ToString(),
                    Dob = req.Dob,
                    ReceiveNewsLetters = req.ReceiveNewsLetters
                });
            _mockMapper?.Setup(m => m.Map<PersonResponseDto>(It.IsAny<Person>()))
                .Returns<Person>((p) => new PersonResponseDto
                {
                    PersonId = p.PersonId,
                    PersonName = p.PersonName,
                    Email = p.Email,
                    Dob = p.Dob,
                    Address = p.Address,
                    CountryId = p.CountryId,
                    Gender = p.Gender,
                    ReceiveNewsLetters = p.ReceiveNewsLetters
                });
            _mockMapper?.Setup(m => m.Map<List<PersonResponseDto>>(It.IsAny<List<Person>>()))
                .Returns<List<Person>>((pList) => pList.Select((pRes) => new PersonResponseDto
                {
                    PersonId = pRes.PersonId,
                    PersonName = pRes.PersonName,
                    Email = pRes.Email,
                    Dob = pRes.Dob,
                    Address = pRes.Address,
                    CountryId = pRes.CountryId,
                    Gender = pRes.Gender,
                    ReceiveNewsLetters = pRes.ReceiveNewsLetters
                }).ToList());

        }

        private void  mockCountryMapper()
        {
            _mockMapper?.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns<CountryAddRequestDto>((cReq) => new Country()
                {
                    CountryName = cReq.CountryName,
                });
            _mockMapper?.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
               .Returns<Country>((c) => new CountryResponseDto()
               {
                   CountryId = c.CountryId,
                   CountryName = c.CountryName
               });
        }

       

        private void  Initializeobjects()
        {
            _countryAddRequestDto =  new CountryAddRequestDto() { CountryName = "Canada" };
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
        public async Task  AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequestDto? personAddRequestDto = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
               await _personsService.AddPerson(personAddRequestDto);
            });


        }

        [Fact]
        public async Task  AddPerson_PersonNameNull()
        {
            //Arrange
            PersonAddRequestDto personAddRequestDto = new PersonAddRequestDto()
            {
                PersonName = null,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
               await _personsService.AddPerson(personAddRequestDto);
            });
        }



        [Fact]
        public async Task  AddPerson_WithProperDetails_ToBeSucessfull()
        {
            //Arrange
            _mockPersonRepository.Setup(x => x.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync( new Person { 
                    PersonId = Guid.NewGuid(),
                    PersonName =_personAddRequestDto?.PersonName,
                    CountryId= _personAddRequestDto?.CountryId,
                    Email = _personAddRequestDto?.Email,
                    Dob = _personAddRequestDto?.Dob,
                    Gender=_personAddRequestDto?.Gender.ToString(),
                    Address = _personAddRequestDto?.Address,
                    ReceiveNewsLetters= _personAddRequestDto.ReceiveNewsLetters,
                    Country=null
                });

            //Act

            PersonResponseDto? actualPersonResponse = await _personsService.AddPerson(_personAddRequestDto);


            //Assert
            Assert.True(actualPersonResponse?.PersonId != Guid.Empty);

        }
        #endregion

        #region GetAllPerson
        [Fact]
        public async Task  GetAllPerson_NullList()
        {
            //Arrange
            _mockPersonRepository.Setup(x => x.GetAllPersons())
                 .ReturnsAsync(new List<Person>());

            //Act
            List<PersonResponseDto>? personResponseDto = await _personsService.GetAllPerson();
            //Assert
            Assert.Empty(personResponseDto);
        }
        [Fact]
        public async Task  GetAllPerson_WithProperdetails()
        {
            //Arrange
            _mockPersonRepository.Setup(x => x.GetAllPersons())
                 .ReturnsAsync(new List<Person>()
                 { 
                     new Person(){ PersonId=Guid.NewGuid()},
                     new Person(){PersonId=Guid.NewGuid() },
                 });
          
            //Act
            List<PersonResponseDto>? personResponseList = await _personsService.GetAllPerson();
            //Assert
            Assert.NotNull(personResponseList);
        }
        #endregion

        #region GetPersonById
        public async Task  GetPersonById_NullPersonId()
        {
            // Arrange
            Guid? personID = null;
            // Act
            PersonResponseDto? personResponseDto = await _personsService.GetPersonById(personID);
            // Assert
            Assert.Null(personResponseDto);

        }
        [Fact]
        public async Task  GetPersonById_WithProperId()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            _mockPersonRepository.Setup(x => x.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(new Person()
                {
                    PersonId = guid,
                    PersonName = "Kokash",
                    Email = "Aakash@mail.com",
                    Address = "Aakash Address",
                    CountryId = Guid.NewGuid(),
                    Gender = "Male",
                    Dob = DateTime.Parse("1996-5-13"),
                    ReceiveNewsLetters = true
                });

             // Act
             PersonResponseDto? actualPersonResponse = await _personsService.GetPersonById(guid);
            // Assert
            Assert.NotNull(actualPersonResponse);
            Assert.True(actualPersonResponse.PersonId == guid);

        }

        #endregion

        #region GetPersonFiltered
        [Fact]
        public async Task  GetFilteredPerson_EmptySearchTest()
        {
            //Arrange
            List<Person> persons = new List<Person>()
                {
                     new Person(){ PersonId=Guid.NewGuid()},
                     new Person(){PersonId=Guid.NewGuid() },
                };
            List<PersonResponseDto> personsResponse = new List<PersonResponseDto>();
            personsResponse.Add(new PersonResponseDto() { PersonId= persons[0].PersonId });
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[1].PersonId });

            _mockPersonRepository.Setup(x => x.GetAllPersons())
                .ReturnsAsync(persons);

            

            //Act

            List<PersonResponseDto> actualPersonResponseList= 
               await _personsService.GetPersonByFilter(nameof(Person.PersonName),"");

            // Assert
            Assert.NotNull(actualPersonResponseList);
            foreach (var person in personsResponse)
            {
                Assert.Contains(person, actualPersonResponseList);
            }

        }

        [Fact]
        public async Task  GetFilteredPerson_SearchByPersonName()
        {
            //Arrange

            List<Person> persons = new List<Person>()
                {
                     new Person(){ PersonId=Guid.NewGuid(),PersonName="Vishnu"},
                     new Person(){PersonId=Guid.NewGuid(),PersonName="Deva"},
                };
            List<PersonResponseDto> personsResponse = new List<PersonResponseDto>();
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[0].PersonId , PersonName = persons[0].PersonName });
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[1].PersonId, PersonName = persons[1].PersonName });

            _mockPersonRepository.Setup(x => x.GetAllPersons())
                .ReturnsAsync(persons);


            //Act

            List<PersonResponseDto> actualPersonResponseList =
               await _personsService.GetPersonByFilter(nameof(Person.PersonName), "v");

            // Assert

            foreach (var person in personsResponse)
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
        public async Task  GetSortedPerson_ByPersonName_Desc()
        {
            //Arrange

            List<Person> persons = new List<Person>()
                {
                     new Person(){ PersonId=Guid.NewGuid(),PersonName="Vishnu"},
                     new Person(){PersonId=Guid.NewGuid(),PersonName="Deva"},
                };
            List<PersonResponseDto> personsResponse = new List<PersonResponseDto>();
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[0].PersonId, PersonName = persons[0].PersonName });
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[1].PersonId, PersonName = persons[1].PersonName });


            List<PersonResponseDto> expectedPersonResponseList=
                personsResponse.OrderByDescending(person => person.PersonName).ToList();


            //Act

            List<PersonResponseDto> actualPersonResponseList =
                _personsService.GetSortedPersons(personsResponse, nameof(Person.PersonName), SortOderOption.DESC);

            // Assert

            for (int i = 0; i < expectedPersonResponseList.Count; i++)
            {
                Assert.Equal(expectedPersonResponseList[i], actualPersonResponseList[i]);
            }




        }

        [Fact]
        public async Task  GetSortedPerson_ByPersonName_Asc()
        {
            //Arrange

            List<Person> persons = new List<Person>()
                {
                     new Person(){ PersonId=Guid.NewGuid(),PersonName="Vishnu"},
                     new Person(){PersonId=Guid.NewGuid(),PersonName="Deva"},
                };
            List<PersonResponseDto> personsResponse = new List<PersonResponseDto>();
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[0].PersonId, PersonName = persons[0].PersonName });
            personsResponse.Add(new PersonResponseDto() { PersonId = persons[1].PersonId, PersonName = persons[1].PersonName });
            List<PersonResponseDto> expectedPersonResponseList =
                personsResponse.OrderBy(person => person.PersonName).ToList();


            //Act

            List<PersonResponseDto> actualPersonResponseList =
                _personsService.GetSortedPersons(personsResponse, nameof(Person.PersonName), SortOderOption.ASC);

            // Assert

            for (int i = 0; i < expectedPersonResponseList.Count; i++)
            {
                Assert.Equal(expectedPersonResponseList[i], actualPersonResponseList[i]);
            }




        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task  UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequestDto personUpdateRequestDto = null;

            
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async() => {
                // Act
               await _personsService.UpdatePerson(personUpdateRequestDto);
            });

        }
        [Fact]
        public async Task  UpdatePerson_InvalidPersonId()
        {
            // Arrange
            PersonUpdateRequestDto personUpdateRequestDto = new PersonUpdateRequestDto() {PersonId=Guid.NewGuid() };
            _mockPersonRepository.Setup(m=>m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Person);


            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                // Act
               await _personsService.UpdatePerson(personUpdateRequestDto);
            });

        }
       
        [Fact]
        public async Task  UpdatePerson_WithProperDetails()
        {
            // Arrange
            Guid personId = Guid.NewGuid();
            PersonUpdateRequestDto personUpdateRequestDto = new PersonUpdateRequestDto() 
            {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = Gender.Male,
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            Person person = new Person() {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            PersonResponseDto personResponseDto = new PersonResponseDto()
            {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            _mockPersonRepository.Setup(m => m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _mockPersonRepository.Setup(m => m.UpdatePerson(It.IsAny<Person>()))
                .ReturnsAsync(person);


            // Act
            PersonResponseDto? actualPersonResponse = await _personsService.UpdatePerson(_personUpdateRequestDto);

            // Assert
            Assert.Equal(personResponseDto, actualPersonResponse);

        }
        #endregion


        [Fact]
        public async Task  DeletePerson_WithoutProperDetails()
        {
            //Arrange
             Guid personId= Guid.NewGuid();
            _mockPersonRepository.Setup(m => m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Person);
            //Act
            bool isdeleted = await _personsService.DeletePerson(personId);

            //Assert
            Assert.False(isdeleted);
        }

        [Fact]
        public async Task  DeletePerson_WithProperDetails()
        {
            //Arrange
            Guid personId = Guid.NewGuid();
            Person person = new Person()
            {
                PersonId = personId,
                PersonName = "Kokash",
                Email = "Aakash@mail.com",
                Address = "Aakash Address",
                CountryId = null,
                Gender = "Male",
                Dob = DateTime.Parse("1996-5-13"),
                ReceiveNewsLetters = true
            };
            _mockPersonRepository.Setup(m => m.GetPersonById(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            _mockPersonRepository.Setup(m => m.DeletePersonById(It.IsAny<Guid>()))
                .ReturnsAsync(true);


            //Act
            bool isdeleted =await _personsService.DeletePerson(person.PersonId);

            //Assert
            Assert.True(isdeleted); 
        }
    }
}
