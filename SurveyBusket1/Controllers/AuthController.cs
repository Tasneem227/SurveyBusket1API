using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBusket1.Contracts.Authentication;
using SurveyBusket8.Contracts.Authentication;
using SurveyBusket8.Services;

namespace SurveyBusket8.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IConfiguration configuration,
                            IOptions<JwtOptions> JwtOptions) : ControllerBase
{
    private readonly IAuthService _AuthService = authService;
    private readonly IConfiguration _Configuration = configuration;
    private readonly JwtOptions _JwtOptions = JwtOptions.Value;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody]LoginRequestUser loginRequest,CancellationToken cancellationToken)
    {
        var authResult= await _AuthService.GetTokenAsync(loginRequest.Email, loginRequest.Password, cancellationToken);
        
            return authResult is null ? BadRequest("Invalid Email or password"):Ok(authResult);

    }
    [HttpPost("Refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody]RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var authResult = await _AuthService.GetRefreshTokenAsync(refreshTokenRequest.Token,refreshTokenRequest.RefreshToken, cancellationToken);

        return authResult is null ? BadRequest("Invalid Token") : Ok(authResult);

    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        bool isRevoked = await _AuthService.RevokeRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

        return isRevoked ?Ok(): BadRequest("operation failed") ;

    }

    [HttpGet("")]
    public IActionResult Test()
    {
        var _config = new
        {
            mykey = _JwtOptions.key,
            //connectionString = _Configuration["ConnectionStrings:DefaultConnections"],
            //Hello_java = _Configuration["Hello.java"],
            //ASPNETCORE_ENVIRONMENT = _Configuration["ASPNETCORE_ENVIRONMENT"]
        };
        return Ok(_config);
    }

}
