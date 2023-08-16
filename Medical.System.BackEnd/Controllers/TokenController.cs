using Medical.System.Core.Models.DTOs;
using Medical.System.Core.Models.Entities;
using Medical.System.Core.Repositories;
using Medical.System.Core.Repositories.Interfaces;
using Medical.System.Core.Services.Implementations;
using Medical.System.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medical.System.BackEnd.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    public TokenController(ITokenService tokenService, IConfiguration configuration, IUserRepository userRepository)
    {
        TokenService = tokenService;
        Configuration = configuration;
        UserRepository = userRepository;
    }

    public ITokenService TokenService { get; }
    public IConfiguration Configuration { get; }
    public IUserRepository UserRepository { get; }

    [HttpPost("login")] 
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        var token = await TokenService.CreateToken(loginDTO);
        return Ok(token);
    }

    //[HttpPost("validate")]
    //public IActionResult ValidateToken(string token)
    //{
    //    // Utiliza el servicio de token para validar el token
    //    var isValid = TokenService.ValidateToken(token);

    //    if (isValid)
    //    {
    //        return Ok(new { IsValid = true });
    //    }
    //    else
    //    {
    //        return BadRequest(new { IsValid = false, Error = "Token inválido" });
    //    }
    //}

    //[HttpPost("refresh")]
    //public async Task<IActionResult> Refresh(string refreshToken)
    //{
    //    // Obtener el usuario por el token de actualización
    //    var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

    //    if (user == null)
    //    {
    //        return Unauthorized(); // o manejar de otra manera
    //    }

    //    // Generar un nuevo token de acceso
    //    var newJwtToken = GenerateJwtToken(user.Username);

    //    // Generar un nuevo token de actualización
    //    var newRefreshToken = GenerateRefreshToken();

    //    // Actualizar el token de actualización en la base de datos
    //    await _userRepository.SaveRefreshTokenAsync(user.Id, newRefreshToken);

    //    return Ok(new
    //    {
    //        Token = newJwtToken,
    //        RefreshToken = newRefreshToken
    //    });
    //}

}
