using AutoMapper;
using Entities.PersonEntity;
using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Person;

namespace Services.PersonService
{
    public class PersonsServices:IPersonsService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonsServices> _logger;

        public PersonsServices(IMapper mapper,IPersonRepository personRepository,ILogger<PersonsServices> logger) {

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
            await _personRepository.AddPerson(person);
            PersonResponseDto? personResponseDto= await GetPersonById(person.PersonId);
            return personResponseDto;

        }

       

        public async Task<List<PersonResponseDto>?> GetAllPerson()
        {
            _logger.LogInformation("reached GetAllPerson of PersonsServices");
            return _mapper.Map<List<PersonResponseDto>>( await _personRepository.GetAllPersons());
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
            if (personResponseList==null) return null;

           

            matchingPersons=  personResponseList.Where(person => {
                Type type = person.GetType();
                var property= type.GetProperty(searchBy);
                if (property != null && property.PropertyType==typeof(string))
                {
                    var value = property.GetValue(person) as string;
                    return (value!=null && !string.IsNullOrEmpty(value) 
                            && value.Contains(searchString,StringComparison.OrdinalIgnoreCase));
                }
                if (property != null && property.PropertyType == typeof(DateTime))
                {
                    var value =Convert.ToDateTime( property.GetValue(person));
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
            PersonResponseDto? personResponseDto= _mapper.Map<PersonResponseDto>(person);
            return personResponseDto;
        }

        public List<PersonResponseDto>? GetSortedPersons(List<PersonResponseDto> persons, string sortby, SortOderOption order)
        {
            _logger.LogInformation("reached GetSortedPersons of PersonsServices");
            List<PersonResponseDto>? sortedPersons = null;
            if (string.IsNullOrEmpty(sortby)) return null;
            sortedPersons = persons;
           
                if (order == SortOderOption.ASC)
                {
                    sortedPersons = persons.OrderBy(person => {
                        var type = person.GetType();
                        var property=type.GetProperty(sortby);
                        var value = property?.GetValue(person, null);
                        if (value is string val)
                        {
                            return val.ToUpper();
                        }
                        else if (value !=null && value.GetType().IsEnum)
                        {
                            return value.ToString().ToUpper();
                        }
                        return value as IComparable;
                        }).ToList();
                }
                else
                {
                    sortedPersons = persons.OrderByDescending(person =>
                    {
                        var type = person.GetType();
                        var property = type.GetProperty(sortby);
                        var value=property?.GetValue(person, null);
                        if (value is string val)
                        {
                            return val.ToUpper();
                        }
                        else if (value!=null&&value.GetType().IsEnum)
                        {
                            value.ToString().ToUpper();
                        }
                        return value as IComparable;
                    }
                    ).ToList();
                }
            
                
            return sortedPersons;
        }

        public async Task<PersonResponseDto?> UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto)
        {
            if (personUpdateRequestDto == null) throw new ArgumentNullException(nameof(PersonUpdateRequestDto));
            if (string.IsNullOrEmpty(personUpdateRequestDto.PersonName))
                throw new ArgumentException("Name cant be null");

            Person? matchingPerson= await _personRepository.GetPersonById(personUpdateRequestDto.PersonId);
            if (matchingPerson == null) throw new ArgumentException("Person dont exists");

            await _personRepository.UpdatePerson(matchingPerson);

            return _mapper.Map<PersonResponseDto>(matchingPerson);

        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("personId cant be null");
            Person? person =  await _personRepository.GetPersonById(personId.Value);
            if (person == null) return false;
            await _personRepository.DeletePersonById(personId);
            return true;
        }
    }
}
