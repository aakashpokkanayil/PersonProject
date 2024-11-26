using ServiceContracts.DTOs.PersonsDtos;

namespace ServiceContracts.Interfaces.Person
{
    public interface IPersonsAdderService
    {
        Task<PersonResponseDto?> AddPerson(PersonAddRequestDto? personAddRequestDto);

    }
}
