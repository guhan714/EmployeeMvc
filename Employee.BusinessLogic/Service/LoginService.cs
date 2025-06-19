using Employee.BusinessLogic.Interfaces.Repository;
using Employee.BusinessLogic.Interfaces.Service.Domain;
using Employee.BusinessLogic.Interfaces.Service.Security;
using Employee.BusinessLogic.Results;
using Employee.BusinessLogic.Results.Dtos;
using Employee.Common.Message;

namespace Employee.BusinessLogic.Service;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;
    private readonly ICryptographyService _cryptographyService;

    public LoginService(ILoginRepository loginRepository, ICryptographyService cryptographyService)
    {
        _loginRepository = loginRepository;
        _cryptographyService = cryptographyService;
    }

    public async Task<Result<LoginDto>> LoginAsync(string username, string password)
    {
        var loginResult = await _loginRepository.LoginAsync(username);
        
        if (loginResult is null)
            return Result<LoginDto>.Fail(LoginMessages.InvalidUsername);
        
        var verifiedPassword =  _cryptographyService.VerifyPassword(password, loginResult.Password);
        
        if(!verifiedPassword)
            return Result<LoginDto>.Fail(LoginMessages.InvalidPassword);
        
        return Result<LoginDto>.Success(loginResult,LoginMessages.AuthenticationSuccess);
    }
}