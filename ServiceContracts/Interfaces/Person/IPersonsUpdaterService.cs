using ServiceContracts.DTOs.PersonsDtos;

namespace ServiceContracts.Interfaces.Person
{
    public interface IPersonsUpdaterService
    {
        Task<PersonResponseDto?> UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto);
    }
}
