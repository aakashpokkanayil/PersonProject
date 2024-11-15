using Entities.CountryEntity;
using Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.PersonEntity
{
    /// <summary>
    /// Domain Model
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? Dob { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        //unique
        public Guid? CountryId { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }
    }
}
