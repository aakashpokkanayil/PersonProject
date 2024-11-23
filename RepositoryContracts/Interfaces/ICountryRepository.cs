using Entities.CountryEntity;
namespace RepositoryContracts.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> AddCountry(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country?> GetCountryById(Guid? countryId);
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
