using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTOs.CountryDtos
{
    /// <summary>
    /// - From BLL COntroller get the resposnse as CountryAddResponseDto.
    /// </summary>
    public class CountryResponseDto
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        /// <summary>
        /// This method checks whether current object matches with other object of type CountryResponseDto
        /// </summary>
        /// <param name="obj">other object of type CountryResponseDto</param>
        /// <returns>returns true if obth object are equal else retutn false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponseDto)) return false;
            CountryResponseDto countryResponseobj = (CountryResponseDto)obj;
            return CountryId == countryResponseobj.CountryId && CountryName == countryResponseobj.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
