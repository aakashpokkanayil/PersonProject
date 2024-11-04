using AutoMapper;
using Entities.CountryEntity;
using ServiceContracts.DTOs.CountryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
                CreateMap<CountryAddRequestDto, Country>().ReverseMap();
                CreateMap<Country, CountryResponseDto>().ReverseMap();
                CreateMap<List<Country>, List<CountryResponseDto>>().ReverseMap();
        }
    }
}
