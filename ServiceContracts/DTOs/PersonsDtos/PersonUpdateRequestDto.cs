using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTOs.PersonsDtos
{
    public class PersonUpdateRequestDto
    {
        [Required(ErrorMessage = "Person Id Can't be null.")]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "Person name Can't be null.")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email cant be null.")]
        [EmailAddress(ErrorMessage = "Valid email address required.")]
        public string? Email { get; set; }
        public DateTime? Dob { get; set; }
        public Gender? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
