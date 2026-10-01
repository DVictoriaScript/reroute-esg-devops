using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReRoute.Api.Data;
using ReRoute.Api.DTOs;
using ReRoute.Api.Services;

namespace ReRoute.Api.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly ReRouteDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(ReRouteDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var participante = await _context.Participantes
            .FirstOrDefaultAsync(x => x.Email == dto.Email && x.Senha == dto.Senha);

        if (participante == null)
            return Unauthorized(new { mensagem = "Email ou senha inválidos." });

        var expiraEm = DateTime.UtcNow.AddHours(2);
        var token = _tokenService.GerarToken(participante, expiraEm);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Nome = participante.Nome,
            Email = participante.Email,
            TipoPerfil = participante.TipoPerfil,
            ExpiraEm = expiraEm
        });
    }
}