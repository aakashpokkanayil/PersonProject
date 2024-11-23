using AutoMapper;
using Entities.CountryEntity;
using Moq;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.Interfaces.Country;
using Services.CountryService;

namespace ProjectTest.CountryUnitTests
{
    /// <summary>
    /// - We inject CountriesService(BLL) in to CountriesServiceTest.cs for UnitTesting using ICountriesService.cs Interface.
    /// </summary>
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly ICountryRepository _countryRepository;

        public CountriesServiceTest()
        {
           
            _mockMapper = new Mock<IMapper>();
            _mockCountryRepository = new Mock<ICountryRepository>();
            _countryRepository = _mockCountryRepository.Object;
            _countriesService = new CountriesService(_mockMapper.Object, _countryRepository);
        }

        #region AddCountry
        /// <summary>
        /// - when CountryAddRequestDto is null then it should throw ArgumentNullException.
        /// </summary>
        [Fact]
        public async Task AddCountry_NUllCountry()
        {
            // Arrange
            CountryAddRequestDto? countryAddRequestDto = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                    // Act
                   async () => await _countriesService.AddCountry(countryAddRequestDto)
            );
        }

        /// <summary>
        /// - when CountryName is null then it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task AddCountry_CountryNameIsNUll()
        {
            // Arrange
            CountryAddRequestDto? countryAddRequestDto = new CountryAddRequestDto();
            countryAddRequestDto.CountryName = null;

            // Assert
           await Assert.ThrowsAsync<ArgumentException>(
                    // Act
                   async () => await _countriesService.AddCountry(countryAddRequestDto)
            );
        }

        /// <summary>
        /// -  when CountryName is duplicated then it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task  AddCountry_DuplicateCountryName()
        {
            // Arrange
            var guid = Guid.NewGuid();
            CountryAddRequestDto? countryAddRequestDto1 = new CountryAddRequestDto();
            countryAddRequestDto1.CountryName = "USA";
            
            Country country = new Country()
            {
                CountryId = guid,
                CountryName ="USA"
            };
           

            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto c) => new Country { CountryName = c.CountryName });

            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto { CountryId = c.CountryId, CountryName = c.CountryName });

            _mockCountryRepository.Setup(m=>m.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(country);



            // Assert
            await Assert.ThrowsAsync<ArgumentException>(
                    // Act
                   async () =>
                    {
                      await  _countriesService.AddCountry(countryAddRequestDto1);
                    }
            );
        }

        /// <summary>
        /// - when correct CountryName provided, it should be inserted in to list of countries or Country table.
        /// </summary>
        [Fact]
        public async Task  AddCountry_SucessfulCountryInsertion()
        {

            // Arrange
            CountryAddRequestDto? expectedCountry = new CountryAddRequestDto();
            expectedCountry.CountryName = "Japan";
            Country country = new Country()
            {
                CountryId = Guid.NewGuid(),
                CountryName = "Japan"
            };



            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto c) => new Country() { CountryName = c.CountryName });
            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto { CountryId = c.CountryId, CountryName = c.CountryName });
            _mockMapper.Setup(m => m.Map<List<CountryResponseDto>>(It.IsAny<List<Country>>()))
                .Returns((List<Country> countries) =>
                    countries.Select((country) => new CountryResponseDto() { CountryId = country.CountryId, CountryName = country.CountryName }).ToList()
                );
            //_mapperMock.Setup(m => m.Map<DestinationType>(It.IsAny<SourceType>()))
            _mockCountryRepository.Setup(m => m.GetCountryByCountryName(It.IsAny<string>()))
              .ReturnsAsync(null as Country);
            _mockCountryRepository.Setup(m => m.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);

            // Act
            CountryResponseDto actualCountry = await _countriesService.AddCountry(expectedCountry);

            // Assert
            Assert.True(actualCountry.CountryId != Guid.Empty);
            Assert.Equal(expectedCountry.CountryName, actualCountry.CountryName);
            // CountryResponseDto to compair this we have to override Equal method in CountryResponseDto.
        }
        #endregion

        #region GetAllCountries
        /// <summary>
        /// Before inserting any country list must be empty.
        /// </summary>
        [Fact]
        public async Task  GetAllCountries_EmptyList()
        {
            //Arrange
            List<Country> countries= new List<Country>();
            _mockMapper.Setup(m => m.Map<List<CountryResponseDto>>(It.IsAny<List<Country>>()))
                .Returns((List<Country> countries) =>
                   countries.Select((country) => new CountryResponseDto { CountryId = country.CountryId, CountryName = country.CountryName }).ToList()
                );
            _mockCountryRepository.Setup(m => m.GetAllCountries())
               .ReturnsAsync(countries);
            //Act
            List<CountryResponseDto> actualCountryResponse = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponse);
        }

        /// <summary>
        /// Countries added and returns must be same.
        /// </summary>
        [Fact]
        public async Task  GetAllCountries_AddAndReturnSameCountries()
        {
            //Arrange
            List<CountryAddRequestDto> countryAddRequest = new List<CountryAddRequestDto>() {
                new CountryAddRequestDto(){ CountryName="India"},
                new CountryAddRequestDto(){ CountryName="Japan"}
            };
            List<Country> countries = new List<Country>()
            {
                new Country()
                {
                    CountryId = Guid.NewGuid(),
                    CountryName = "India"
                },
                new Country()
                {
                    CountryId = Guid.NewGuid(),
                    CountryName = "Japan"
                }
            };

            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto cr) => new Country() { CountryName = cr.CountryName });

            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto() { CountryId = c.CountryId, CountryName = c.CountryName });
            //(Country c) ==> is the same parameter passing mapping in service class AddCountry method
            // refer this in service class AddCountry method:
            // CountryResponseDto countryAddResponseDto = _mapper.Map<CountryResponseDto>(country);

            List<CountryResponseDto> expectedCountryResponse = new List<CountryResponseDto>();
            _mockMapper.Setup(m => m.Map<List<CountryResponseDto>>(It.IsAny<List<Country>>()))
                .Returns((List<Country> countries) =>
                    countries.Select(c => new CountryResponseDto() { CountryId = c.CountryId, CountryName = c.CountryName }).ToList());

            //countries.Select(...) iterates over each Country in the countries list.
            //For each Country (c), we create a new CountryResponseDto object with CountryId and
            //CountryName properties copied from Country to CountryResponseDto
            //The purpose of this Select is to map each Country in the list to a CountryResponseDto
            //mapping in service class GetAllCountries() method :
            //List<CountryResponseDto> countryResponseList = _mapper.Map<List<CountryResponseDto>>(_countries);

            //Act
            _mockCountryRepository.Setup(m=>m.GetAllCountries())
                .ReturnsAsync(countries);
            

            List<CountryResponseDto> actualcountryResponse =await _countriesService.GetAllCountries();


            Assert.Contains(actualcountryResponse, x => x.CountryName == "India");
            Assert.Contains(actualcountryResponse, x => x.CountryName == "Japan");



        }
        #endregion

        #region GetCountryById
        /// <summary>
        /// If supplied CountryId is null,method should return null.
        /// </summary>
        [Fact]
        public async Task  GetCountryById_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponseDto? countryResponse = await _countriesService.GetCountryById(countryId);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public async Task  GetCountryById_ValidCountryId()
        {
            //Arrange
            Guid countryId = Guid.NewGuid();
            CountryAddRequestDto countryAddRequest = new CountryAddRequestDto()
            {
                CountryName = "India",
            };
            Country country = new Country()
            {
                CountryId = countryId,
                CountryName = "Japan"
            };
            CountryResponseDto expectedcountryResponse = new CountryResponseDto()
            {
                CountryId = countryId,
                CountryName = "Japan"
            };
            _mockCountryRepository.Setup(m=>m.GetCountryById(It.IsAny<Guid>())).ReturnsAsync(country);
            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto c) => new Country { CountryName = c.CountryName });
            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto { CountryId = c.CountryId, CountryName = c.CountryName });

            //Act
            CountryResponseDto? actualCountryResponse = await _countriesService.GetCountryById(countryId);

            //Assert
            Assert.Equal(expectedcountryResponse, actualCountryResponse);
        }
        #endregion

    }
}
