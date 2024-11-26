namespace ServiceContracts.Interfaces.Person
{
    public interface IPersonsDeleterService
    {
        Task<bool> DeletePerson(Guid? personId);

    }
}
