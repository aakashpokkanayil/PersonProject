using System;

namespace Entities.CountryEntity
{
    /// <summary>
    /// - This is a Domain Model(DOmain model and BLL are diff layer), 
    ///   Domain Model is accessible to the BLL and the DAL but is not directly exposed to the Presentation Layer(view and controller).
    ///   
    /// - We can use DTOs to Transfer data between Domain model and Presentation Layer(view and controller).
    ///   means from controller -->(req dto)-->BLL(map req dto to DOmain model)-->
    ///   (After DB Insertion get resposnse as Domain Model)-->BLL Maps DOmain Model toresposnseDto
    ///   --> BLL send response as (resposnseDto) to controller-->Controller(maps to ViewModel)
    ///   
    /// - In EF Core this DomainModels are consider as DB source.
    /// 
    /// - Here controller/Unit Test invoke AddCountries(in BLL) , controller pass object as CountryAddRequestDTO  
    ///   and receives response as CountryResponseDTO Then CountryResponseDTO is convert to ViewModel(MVC) or CountryResponseDTO to client(API)
    /// </summary>
    public class Country
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

    }
}
