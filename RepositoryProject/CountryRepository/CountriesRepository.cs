using Entities.CountryEntity;
using Microsoft.EntityFrameworkCore;
using PersonData.PersonsContext;
using RepositoryContracts.Interfaces;

namespace RepositoryProject.CountryRepository
{
    public class CountriesRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CountriesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();  
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _dbContext.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            return await _dbContext.Countries.FirstOrDefaultAsync(x=>x.CountryName == countryName);
        }

        public async Task<Country?> GetCountryById(Guid? countryId)
        {
            return await _dbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == countryId);
        }
    }
}
