using Employee.BusinessLogic.Interfaces.Repository;
using Employee.BusinessLogic.Results.Dtos;
using Employee.DataAccess.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Employee.DataAccess.Persistence.Repository.Domain;

public class LoginRepository : ILoginRepository
{
    private readonly AppDbContext _context;

    public LoginRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LoginDto?> LoginAsync(string username)
    {
        var user = await _context.Users.Where(s => s.Email == username)
            .Select(a => new LoginDto
            (
                a.UserId,
                a.FirstName + " " + a.LastName,
                a.Password
            ))
            .FirstOrDefaultAsync();
        
        return user;
    }
}