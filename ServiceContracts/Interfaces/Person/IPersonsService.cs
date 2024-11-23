using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;

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
