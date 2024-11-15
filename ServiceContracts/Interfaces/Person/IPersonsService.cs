using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Interfaces.Person
{
    public interface IPersonsService
    {
        Task<PersonResponseDto?> AddPerson(PersonAddRequestDto? personAddRequestDto);
        Task<List<PersonResponseDto>?> GetAllPerson();
        Task<PersonResponseDto?> GetPersonById(Guid? personId);
        Task<List<PersonResponseDto>?> GetPersonByFilter(string searchBy,string? searchString);
        List<PersonResponseDto>? GetSortedPersons(List<PersonResponseDto> persons,string sortby,SortOderOption order);
        Task<PersonResponseDto?> UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto);

        Task<bool> DeletePerson(Guid? personId);


    }
}
