using AutoMapper;
using Entities.PersonEntity;
using Microsoft.Extensions.Logging;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Interfaces.Person;

namespace Services.PersonService
{
    public class PersonsAdderServices:IPersonsAdderService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonsAdderServices> _logger;

        public PersonsAdderServices(IMapper mapper,IPersonRepository personRepository,ILogger<PersonsAdderServices> logger) {

            _mapper = mapper;
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<PersonResponseDto?> AddPerson(PersonAddRequestDto? personAddRequestDto)
        {
            if (personAddRequestDto == null) throw new ArgumentNullException(nameof(personAddRequestDto));
            if (String.IsNullOrEmpty(personAddRequestDto.PersonName)) 
                throw new ArgumentException("PersonName Can't be null");
            Person person= _mapper.Map<Person>(personAddRequestDto);
            person.PersonId=Guid.NewGuid();
            person = await _personRepository.AddPerson(person);
            return _mapper.Map<PersonResponseDto>(person);
        }

    }
}
