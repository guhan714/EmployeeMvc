using Employee.BusinessLogic.Interfaces.Service.Masters;
using Employee.DataAccess.Persistence.DbContexts;
using Employee.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Persistence.Repository.Masters;

public class RoleService : IRoleService
{
    private readonly AppDbContext context;

    public RoleService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<UserRole[]> GetUserRolesAsync(Guid id)
    {
        var roles = await context.UserRoles
            .Include(a => a.User)
            .Include(r => r.Role)
            .AsSplitQuery()
            .Where(a => a.UserId == id)
            .ToArrayAsync();
        return roles;
    }
}