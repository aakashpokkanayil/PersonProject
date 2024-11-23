using AutoMapper;
using Entities.CountryEntity;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.Interfaces.Country;

namespace Services.CountryService
{
    public class CountriesService : ICountriesService
    {
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CountriesService(IMapper mapper,ICountryRepository countryRepository)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        #region AddCountry
        /// <summary>
        /// Add Countries to database via repository design pattern.
        /// </summary>
        /// <param name="countryAddRequestDto">Gives Country details to this method</param>
        /// <returns>returns Inserted Country with Id as CountryAddResponseDto</returns>
        public async Task<CountryResponseDto> AddCountry(CountryAddRequestDto? countryAddRequestDto)
        {
            //Validation: countryAddRequestDto cant be null
            if (countryAddRequestDto == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequestDto));
            }
            //Validation: CountryName cant be null
            if (countryAddRequestDto.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequestDto.CountryName));
            }
            //Validation: Duplicate CountryName cant exists.
            if (await _countryRepository.GetCountryByCountryName(countryAddRequestDto.CountryName)!=null)
            {
                throw new ArgumentException("Given country name already exists.");
            }

            // mapping CountryAddRequestDto to Domain model Country
            Country country = _mapper.Map<Country>(countryAddRequestDto);
            // generate Guid for CountryId
            country.CountryId = Guid.NewGuid();
             await _countryRepository.AddCountry(country);
            // mapping Domain model Country to CountryAddResponseDto
            CountryResponseDto countryAddResponseDto = _mapper.Map<CountryResponseDto>(country);
            return countryAddResponseDto;

        }

        #endregion

        #region GetCountries
        public async Task<List<CountryResponseDto>> GetAllCountries()
        {
            List<CountryResponseDto> countryResponseList = 
                _mapper.Map<List<CountryResponseDto>>( await _countryRepository.GetAllCountries());
            return countryResponseList;
        }

        public async Task<CountryResponseDto?> GetCountryById(Guid? CountryId)
        {
            if (CountryId == null) return null;
            Country? country = await _countryRepository.GetCountryById(CountryId);
            if (country == null) return null;
            return _mapper.Map<CountryResponseDto>(country);
        }

        #endregion
    }
}
