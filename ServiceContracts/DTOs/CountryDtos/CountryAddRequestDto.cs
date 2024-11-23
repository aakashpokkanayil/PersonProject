namespace ServiceContracts.DTOs.CountryDtos
{
    /// <summary>
    /// - Client sends request in the form of CountryAddRequestDto
    /// - From CLient we need only CountryName so this DTO only need CountryName.
    /// - when adding to DB, CountryId should generate automatically, its not the one client provides.
    /// </summary>
    public class CountryAddRequestDto
    {
        public string? CountryName { get; set; }
    }
}
