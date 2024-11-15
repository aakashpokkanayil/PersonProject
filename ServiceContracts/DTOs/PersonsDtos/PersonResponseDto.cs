using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTOs.PersonsDtos
{
    public class PersonResponseDto
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? Dob { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType()!=typeof(PersonResponseDto)) return false;
            PersonResponseDto personResponseDto = (PersonResponseDto)obj;
            return (PersonId == personResponseDto.PersonId && PersonName == personResponseDto.PersonName &&
                    Email == personResponseDto.Email && Dob == personResponseDto.Dob && Gender == personResponseDto.Gender
                    && CountryId == personResponseDto.CountryId && Address == personResponseDto.Address &&
                    ReceiveNewsLetters == personResponseDto.ReceiveNewsLetters);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
