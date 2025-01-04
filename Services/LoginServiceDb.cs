using Microsoft.Extensions.Logging;
using DbRepos;
using Models.DTO;

namespace Services;

public class LoginServiceDb : ILoginService
{
    private readonly LoginDbRepos _repo;
    private readonly ILogger<LoginServiceDb> _logger;

    public LoginServiceDb(ILogger<LoginServiceDb> logger, LoginDbRepos repo)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<ResponseItemDto<LoginUserSessionDto>> LoginUserAsync(LoginCredentialsDto usrCreds)
    {
        try
        {
            var _usrSession = await _repo.LoginUserAsync(usrCreds);
            return _usrSession;
        }
        catch
        {
            //if there was an error during login, simply pass it on.
            throw;
        }
    }
}

