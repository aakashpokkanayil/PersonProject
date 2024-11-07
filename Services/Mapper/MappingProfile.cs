using AutoMapper;
using Entities.CountryEntity;
using Entities.PersonEntity;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.DTOs.PersonsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Country
            CreateMap<CountryAddRequestDto, Country>().ReverseMap();
            CreateMap<Country, CountryResponseDto>().ReverseMap();
            CreateMap<List<Country>, List<CountryResponseDto>>().ReverseMap();
            #endregion

            #region Person
            CreateMap<PersonAddRequestDto,Person>()
                .ReverseMap();
            CreateMap<Person, PersonResponseDto>()
                .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>(DateTime.Now.Year- Convert.ToDateTime(src.Dob).Year)))
                .ReverseMap();
            CreateMap<List<Person>, List<PersonResponseDto>>().ReverseMap();

            CreateMap<PersonUpdateRequestDto, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            #endregion
        }
    }
}
