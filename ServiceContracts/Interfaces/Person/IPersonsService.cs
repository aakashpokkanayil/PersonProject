﻿using ServiceContracts.DTOs.PersonsDtos;
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
        PersonResponseDto? AddPerson(PersonAddRequestDto? personAddRequestDto);
        List<PersonResponseDto>? GetAllPerson();
        PersonResponseDto? GetPersonById(Guid? personId);
        List<PersonResponseDto>? GetPersonByFilter(string searchBy,string? searchString);
        List<PersonResponseDto>? GetSortedPersons(List<PersonResponseDto> persons,string sortby,SortOderOption order);
        PersonResponseDto? UpdatePerson(PersonUpdateRequestDto? personUpdateRequestDto);

        bool DeletePerson(Guid? personId);


    }
}
