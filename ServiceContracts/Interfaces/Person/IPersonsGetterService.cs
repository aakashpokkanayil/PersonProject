using ServiceContracts.DTOs.PersonsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Interfaces.Person
{
    public interface IPersonsGetterService
    {
        Task<List<PersonResponseDto>?> GetAllPerson();
        Task<PersonResponseDto?> GetPersonById(Guid? personId);
        Task<List<PersonResponseDto>?> GetPersonByFilter(string searchBy, string? searchString);
    }
}
