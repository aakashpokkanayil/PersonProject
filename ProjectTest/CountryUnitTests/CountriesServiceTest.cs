using AutoMapper;
using Entities.CountryEntity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.Interfaces.Country;
using Services.CountryService;
using Services.Mapper;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace ProjectTest.CountryUnitTests
{
    /// <summary>
    /// - We inject CountriesService(BLL) in to CountriesServiceTest.cs for UnitTesting using ICountriesService.cs Interface.
    /// </summary>
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly Mock<IMapper> _mockMapper;

        public CountriesServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _countriesService = new CountriesService(_mockMapper.Object);
        }

        #region AddCountry
        /// <summary>
        /// - when CountryAddRequestDto is null then it should throw ArgumentNullException.
        /// </summary>
        [Fact]
        public void AddCountry_NUllCountry()
        {
            // Arrange
            CountryAddRequestDto? countryAddRequestDto = null;

            // Assert
            Assert.Throws<ArgumentNullException>(
                    // Act
                    () => _countriesService.AddCountry(countryAddRequestDto)
            );
        }

        /// <summary>
        /// - when CountryName is null then it should throw ArgumentException.
        /// </summary>
        [Fact]
        public void AddCountry_CountryNameIsNUll()
        {
            // Arrange
            CountryAddRequestDto? countryAddRequestDto = new CountryAddRequestDto();
            countryAddRequestDto.CountryName = null;

            // Assert
            Assert.Throws<ArgumentException>(
                    // Act
                    () => _countriesService.AddCountry(countryAddRequestDto)
            );
        }

        /// <summary>
        /// -  when CountryName is duplicated then it should throw ArgumentException.
        /// </summary>
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            // Arrange
            var guid = Guid.NewGuid();
            CountryAddRequestDto? countryAddRequestDto1 = new CountryAddRequestDto();
            countryAddRequestDto1.CountryName = "USA";

            CountryAddRequestDto? countryAddRequestDto2 = new CountryAddRequestDto();
            countryAddRequestDto2.CountryName = "USA";

            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto c) => new Country { CountryName = c.CountryName });

            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto { CountryId = c.CountryId, CountryName = c.CountryName });

            // Assert
            Assert.Throws<ArgumentException>(
                    // Act
                    () =>
                    {
                        _countriesService.AddCountry(countryAddRequestDto1);
                        _countriesService.AddCountry(countryAddRequestDto2);
                    }
            );
        }

        /// <summary>
        /// - when correct CountryName provided, it should be inserted in to list of countries or Country table.
        /// </summary>
        [Fact]
        public void AddCountry_SucessfulCountryInsertion()
        {

            // Arrange
            CountryAddRequestDto? expectedCountry = new CountryAddRequestDto();
            expectedCountry.CountryName = "Japan";



            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto c) => new Country() { CountryName = c.CountryName });
            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto { CountryId = c.CountryId, CountryName = c.CountryName });
            _mockMapper.Setup(m => m.Map<List<CountryResponseDto>>(It.IsAny<List<Country>>()))
                .Returns((List<Country> countries) =>
                    countries.Select((country) => new CountryResponseDto() { CountryId = country.CountryId, CountryName = country.CountryName }).ToList()
                );
            //_mapperMock.Setup(m => m.Map<DestinationType>(It.IsAny<SourceType>()))

            // Act
            CountryResponseDto actualCountry = _countriesService.AddCountry(expectedCountry);
            List<CountryResponseDto> actualResponseList = _countriesService.GetAllCountries();

            // Assert
            Assert.True(actualCountry.CountryId != Guid.Empty);
            Assert.Equal(expectedCountry.CountryName, actualCountry.CountryName);
            Assert.Contains(actualCountry, actualResponseList);
            // CountryResponseDto to compair this we have to override Equal method in CountryResponseDto.
        }
        #endregion

        #region GetAllCountries
        /// <summary>
        /// Before inserting any country list must be empty.
        /// </summary>
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Arrange
            _mockMapper.Setup(m => m.Map<List<CountryResponseDto>>(It.IsAny<List<Country>>()))
                .Returns((List<Country> countries) =>
                   countries.Select((country) => new CountryResponseDto { CountryId = country.CountryId, CountryName = country.CountryName }).ToList()
                );
            //Act
            List<CountryResponseDto> actualCountryResponse = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponse);
        }

        /// <summary>
        /// Countries added and returns must be same.
        /// </summary>
        [Fact]
        public void GetAllCountries_AddAndReturnSameCountries()
        {
            //Arrange
            List<CountryAddRequestDto> countryAddRequest = new List<CountryAddRequestDto>() {
                new CountryAddRequestDto(){ CountryName="India"},
                new CountryAddRequestDto(){ CountryName="Japan"}
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
            foreach (var item in countryAddRequest)
            {

                expectedCountryResponse.Add(_countriesService.AddCountry(item));
            }

            List<CountryResponseDto> actualcountryResponse = _countriesService.GetAllCountries();

            //Assert
            foreach (var expected_country in expectedCountryResponse)
            {
                Assert.Contains(expected_country, actualcountryResponse);
                // CountryResponseDto to compair this we have to override Equal method in CountryResponseDto.
            }

            Assert.Contains(actualcountryResponse, x => x.CountryName == "India");
            Assert.Contains(actualcountryResponse, x => x.CountryName == "Japan");



        }
        #endregion

        #region GetCountryById
        /// <summary>
        /// If supplied CountryId is null,method should return null.
        /// </summary>
        [Fact]
        public void GetCountryById_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponseDto? countryResponse = _countriesService.GetCountryById(countryId);

            //Assert
            Assert.Null(countryResponse);
        }

        [Fact]
        public void GetCountryById_ValidCountryId()
        {
            //Arrange
            CountryAddRequestDto countryAddRequest = new CountryAddRequestDto()
            {
                CountryName = "India",
            };
            _mockMapper.Setup(m => m.Map<Country>(It.IsAny<CountryAddRequestDto>()))
                .Returns((CountryAddRequestDto c) => new Country { CountryName = c.CountryName });
            _mockMapper.Setup(m => m.Map<CountryResponseDto>(It.IsAny<Country>()))
                .Returns((Country c) => new CountryResponseDto { CountryId = c.CountryId, CountryName = c.CountryName });
            CountryResponseDto expectedcountryResponse = _countriesService.AddCountry(countryAddRequest);

            //Act
            CountryResponseDto? actualCountryResponse = _countriesService.GetCountryById(expectedcountryResponse.CountryId);

            //Assert
            Assert.Equal(expectedcountryResponse, actualCountryResponse);
        }
        #endregion

    }
}
