using Employee.Domain.Models;

namespace Employee.BusinessLogic.Interfaces.Service.Masters;

public interface IRoleService
{
    Task<UserRole[]> GetUserRolesAsync(Guid id);
}