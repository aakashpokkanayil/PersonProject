using AutoMapper;
using Entities.PersonEntity;
using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Interfaces.Person;

namespace Services.PersonService
{
    public class PersonsGetterServices: IPersonsGetterService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonsAdderServices> _logger;

        public PersonsGetterServices(IMapper mapper, IPersonRepository personRepository, ILogger<PersonsAdderServices> logger)
        {

            _mapper = mapper;
            _personRepository = personRepository;
            _logger = logger;
        }
        public async Task<List<PersonResponseDto>?> GetAllPerson()
        {
            _logger.LogInformation("reached GetAllPerson of PersonsServices");
            return _mapper.Map<List<PersonResponseDto>>(await _personRepository.GetAllPersons());
        }

        public async Task<List<PersonResponseDto>?> GetPersonByFilter(string searchBy, string? searchString)
        {
            _logger.LogInformation("reached GetPersonByFilter of PersonsServices");

            List<PersonResponseDto>? personResponseList = await GetAllPerson();
            List<PersonResponseDto>? matchingPersons = personResponseList;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }
            if (personResponseList == null) return null;



            matchingPersons = personResponseList.Where(person => {
                Type type = person.GetType();
                var property = type.GetProperty(searchBy);
                if (property != null && property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(person) as string;
                    return (value != null && !string.IsNullOrEmpty(value)
                            && value.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                }
                if (property != null && property.PropertyType == typeof(DateTime))
                {
                    var value = Convert.ToDateTime(property.GetValue(person));
                    return (value.ToString("dd MMM yyy").Contains(searchString, StringComparison.OrdinalIgnoreCase));
                }
                return false;

            }).ToList();
            return matchingPersons;
        }

        public async Task<PersonResponseDto?> GetPersonById(Guid? personId)
        {
            if (personId == null) return null;
            Person? person = await _personRepository.GetPersonById(personId.Value);
            if (person == null) throw new InvalidPersonIdException("Given Person Id doesnot exists");
            PersonResponseDto? personResponseDto = _mapper.Map<PersonResponseDto>(person);
            return personResponseDto;
        }
    }
}
