using Microsoft.Extensions.Logging;
using DbRepos;
using Models.DTO;

namespace Services;

public class LoginServiceDb : ILoginService
{
    private readonly LoginDbRepos _repo;
    private readonly JWTService _jtwService;

    private readonly ILogger<LoginServiceDb> _logger;

    public LoginServiceDb(ILogger<LoginServiceDb> logger, LoginDbRepos repo, JWTService jtwService)
    {
        _repo = repo;
        _jtwService = jtwService;
        _logger = logger;
    }

    public async Task<ResponseItemDto<LoginUserSessionDto>> LoginUserAsync(LoginCredentialsDto usrCreds)
    {
        try
        {
            var _usrSession = await _repo.LoginUserAsync(usrCreds);

            //Successful login. Create a JWT token
            _usrSession.Item.JwtToken = _jtwService.CreateJwtUserToken(_usrSession.Item);

            //For test only, decypt the JWT token and compare.
            var _tmpUserSession = _jtwService.DecodeToken(_usrSession.Item.JwtToken.EncryptedToken);

            return _usrSession;
        }
        catch
        {
            //if there was an error during login, simply pass it on.
            throw;
        }
    }
}

