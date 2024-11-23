using AutoMapper;
using Entities.CountryEntity;
using Entities.PersonEntity;
using ServiceContracts.DTOs.CountryDtos;
using ServiceContracts.DTOs.PersonsDtos;

namespace Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Country
            CreateMap<CountryAddRequestDto, Country>();
            CreateMap<Country, CountryResponseDto>();
           
            #endregion

            #region Person
            CreateMap<PersonAddRequestDto, Person>();
            CreateMap<Person, PersonResponseDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => (DateTime.Now.Year - Convert.ToDateTime(src.Dob).Year)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country == null ? null : src.Country.CountryName));


            CreateMap<PersonUpdateRequestDto, Person>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            #endregion
        }
    }
}
