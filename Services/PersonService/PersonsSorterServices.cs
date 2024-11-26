using AutoMapper;
using Microsoft.Extensions.Logging;
using RepositoryContracts.Interfaces;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Person;

namespace Services.PersonService
{
    public class PersonsSorterServices:IPersonsSorterService
    {
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonsAdderServices> _logger;

        public PersonsSorterServices(IMapper mapper, IPersonRepository personRepository, ILogger<PersonsAdderServices> logger)
        {

            _mapper = mapper;
            _personRepository = personRepository;
            _logger = logger;
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
                    var property = type.GetProperty(sortby);
                    var value = property?.GetValue(person, null);
                    if (value is string val)
                    {
                        return val.ToUpper();
                    }
                    else if (value != null && value.GetType().IsEnum)
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
                    var value = property?.GetValue(person, null);
                    if (value is string val)
                    {
                        return val.ToUpper();
                    }
                    else if (value != null && value.GetType().IsEnum)
                    {
                        value.ToString().ToUpper();
                    }
                    return value as IComparable;
                }
                ).ToList();
            }


            return sortedPersons;
        }
    }
}
