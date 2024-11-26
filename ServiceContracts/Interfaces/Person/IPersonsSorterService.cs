using ServiceContracts.DTOs.PersonsDtos;
using ServiceContracts.Enums;

namespace ServiceContracts.Interfaces.Person
{
    public interface IPersonsSorterService
    {
        List<PersonResponseDto>? GetSortedPersons(List<PersonResponseDto> persons, string sortby, SortOderOption order);

    }
}
