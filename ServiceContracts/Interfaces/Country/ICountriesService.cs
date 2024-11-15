using ServiceContracts.DTOs.CountryDtos;


namespace ServiceContracts.Interfaces.Country
{
    /// <summary>
    /// - Represents Business Logic for manipulating Country Entity(DB Table) 
    /// - Methods for BLL Countries.
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds Country in to List of Countries (Later in to Country Table.).
        /// </summary>
        /// <param name="countryAddRequestDto">Country object to be added.</param>
        /// <returns>Returns the country object after adding it (Including newly generated country id.).</returns>
        Task<CountryResponseDto> AddCountry(CountryAddRequestDto? countryAddRequestDto);

        /// <summary>
        /// Fetch all countries
        /// </summary>
        /// <returns>returns List<CountryAddResponseDto></returns>
        Task<List<CountryResponseDto>> GetAllCountries();

        /// <summary>
        /// Returns a country object based on countryid
        /// </summary>
        /// <param name="CountryId">CountryId(guid) to search</param>
        /// <returns>Matching country as CountryResponseDto object</returns>
        Task<CountryResponseDto?> GetCountryById(Guid? CountryId);

    }
}
