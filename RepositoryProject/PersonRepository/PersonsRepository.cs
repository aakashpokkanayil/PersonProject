using Entities.PersonEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonData.PersonsContext;
using RepositoryContracts.Interfaces;
using System.Linq.Expressions;

namespace RepositoryProject.PersonRepository
{
    public class PersonsRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<PersonsRepository> _logger;

        public PersonsRepository(ApplicationDbContext dbContext,ILogger<PersonsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _logger.LogInformation("AddPerson method in PersonsRepository");
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonById(Guid? personId)
        {
            _dbContext.Persons.RemoveRange(_dbContext.Persons.Where(x => x.PersonId == personId));
            int rowsDeleted=await _dbContext.SaveChangesAsync();
            return rowsDeleted>0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _dbContext.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _dbContext.Persons.Include("Country")
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid? personId)
        {
            return await _dbContext.Persons
                .Include("Country")
                .FirstOrDefaultAsync(x => x.PersonId == personId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
            if (matchingPerson == null) return person;

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.Dob = person.Dob;
            matchingPerson.Address = person.Address;
            matchingPerson.CountryId = person.CountryId;
            matchingPerson.Gender = person?.Gender?.ToString();
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await _dbContext.SaveChangesAsync();
            return matchingPerson;

        }
    }
}
