using Employee.BusinessLogic.Results.Dtos;

namespace Employee.BusinessLogic.Interfaces.Repository;

public interface ILoginRepository
{
    Task<LoginDto?> LoginAsync(string username);
}