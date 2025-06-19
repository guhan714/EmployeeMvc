using Employee.BusinessLogic.Results;
using Employee.BusinessLogic.Results.Dtos;

namespace Employee.BusinessLogic.Interfaces.Service.Domain;

public interface ILoginService
{
    Task<Result<LoginDto>> LoginAsync(string username, string password);
}