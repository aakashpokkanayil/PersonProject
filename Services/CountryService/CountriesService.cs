using AutoMapper;
using Entities.CountryEntity;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.Interfaces.Country;

namespace Services.CountryService
{
    public class CountriesService : ICountriesService
    {
        private readonly IMapper _mapper;
        private readonly List<Country> _countries;


        public CountriesService(IMapper mapper)
        {
            _mapper = mapper;
            _countries = new List<Country>();
        }

        #region AddCountry
        /// <summary>
        /// Add Countries to database via repository design pattern.
        /// </summary>
        /// <param name="countryAddRequestDto">Gives Country details to this method</param>
        /// <returns>returns Inserted Country with Id as CountryAddResponseDto</returns>
        public CountryResponseDto AddCountry(CountryAddRequestDto? countryAddRequestDto)
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
            if (_countries.Where(x => x.CountryName == countryAddRequestDto.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists.");
            }

            // mapping CountryAddRequestDto to Domain model Country
            Country country = _mapper.Map<Country>(countryAddRequestDto);
            // generate Guid for CountryId
            country.CountryId = Guid.NewGuid();
            _countries.Add(country);
            // mapping Domain model Country to CountryAddResponseDto
            CountryResponseDto countryAddResponseDto = _mapper.Map<CountryResponseDto>(country);
            return countryAddResponseDto;

        }

        #endregion

        #region GetCountries
        List<CountryResponseDto> ICountriesService.GetAllCountries()
        {
            List<CountryResponseDto> countryResponseList = _mapper.Map<List<CountryResponseDto>>(_countries);
            return countryResponseList;
        }

        CountryResponseDto? ICountriesService.GetCountryById(Guid? CountryId)
        {
            if (CountryId == null) return null;
            Country? country = _countries.FirstOrDefault(x => x.CountryId == CountryId);
            if (country == null) return null;

            return _mapper.Map<CountryResponseDto>(country);
        }

        #endregion
    }
}
