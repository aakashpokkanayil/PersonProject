using AutoMapper;
using Entities.PersonEntity;
using Microsoft.Extensions.Logging;
using RepositoryContracts.Interfaces;
using ServiceContracts.Interfaces.Person;

namespace Services.PersonService
{
    public class PersonsDeleterServices:IPersonsDeleterService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonsAdderServices> _logger;

        public PersonsDeleterServices(IMapper mapper, IPersonRepository personRepository, ILogger<PersonsAdderServices> logger)
        {

            _mapper = mapper;
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("personId cant be null");
            Person? person = await _personRepository.GetPersonById(personId.Value);
            if (person == null) return false;
            await _personRepository.DeletePersonById(personId);
            return true;
        }
        
    }
}
