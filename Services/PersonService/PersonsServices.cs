using AutoMapper;
using Entities.CountryEntity;
using Entities.PersonEntity;
using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.PersonService
{
    public class PersonsServices:IPersonsService
    {
        private readonly IMapper _mapper;
        private readonly ICountriesService _countriesService;
        private readonly List<Person> _personList;
        public PersonsServices(IMapper mapper,ICountriesService countriesService) {
            _mapper = mapper;
            _countriesService = countriesService;
            _personList = new List<Person>();
        }

        public PersonResponseDto? AddPerson(PersonAddRequestDto? personAddRequestDto)
        {
            if (personAddRequestDto == null) throw new ArgumentNullException(nameof(personAddRequestDto));
            if (String.IsNullOrEmpty(personAddRequestDto.PersonName)) 
                throw new ArgumentException("PersonName Can't be null");
            Person person= _mapper.Map<Person>(personAddRequestDto);
            person.PersonId=Guid.NewGuid();
            _personList.Add(person);
            PersonResponseDto? personResponseDto= _mapper.Map<PersonResponseDto>(person);
            personResponseDto.Country= _countriesService.GetCountryById(person.CountryId)?.CountryName;
            return personResponseDto;

        }

       

        public List<PersonResponseDto>? GetAllPerson()
        {
            return _mapper.Map<List<PersonResponseDto>>(_personList);
        }

        public List<PersonResponseDto>? GetPersonByFilter(string searchBy, string? searchString)
        {
            List<PersonResponseDto>? personResponseList = GetAllPerson();
            List<PersonResponseDto>? matchingPersons = personResponseList;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            { 
                return matchingPersons;
            }
            if (personResponseList==null) return null;

            matchingPersons=personResponseList.Where(person => {
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

        public PersonResponseDto? GetPersonById(Guid? personId)
        {
            if (personId == null) return null;
            Person? person = _personList.FirstOrDefault(x => x.PersonId == personId);
            if (person == null) return null;
            PersonResponseDto? personResponseDto= _mapper.Map<PersonResponseDto>(person);
            return personResponseDto;
        }

        public List<PersonResponseDto>? GetSortedPersons(List<PersonResponseDto> persons, string sortby, SortOderOption order)
        {
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

        public PersonResponseDto? UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto)
        {
            if (personUpdateRequestDto == null) throw new ArgumentNullException(nameof(PersonUpdateRequestDto));
            if (string.IsNullOrEmpty(personUpdateRequestDto.PersonName))
                throw new ArgumentException("Name cant be null");

            Person? matchingPerson= _personList.FirstOrDefault(p => p.PersonId == personUpdateRequestDto.PersonId);
            if (matchingPerson == null) throw new ArgumentException("Person dont exists");

            matchingPerson.PersonName=personUpdateRequestDto.PersonName;
            matchingPerson.Email=personUpdateRequestDto.Email;
            matchingPerson.Dob = personUpdateRequestDto.Dob;
            matchingPerson.Address = personUpdateRequestDto.Address;
            matchingPerson.CountryId = personUpdateRequestDto.CountryId;
            matchingPerson.Gender = (Entities.Enum.Gender?)personUpdateRequestDto.Gender;
            matchingPerson.ReceiveNewsLetters = personUpdateRequestDto.ReceiveNewsLetters;

            return _mapper.Map<PersonResponseDto>(matchingPerson);

        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId == null) throw new ArgumentNullException("personId cant be null");
            Person? person = _personList.FirstOrDefault(x=>x.PersonId==personId);
            if (person == null) return false;

            _personList.RemoveAll(person=>person.PersonId== personId);
            return true;
        }
    }
}
