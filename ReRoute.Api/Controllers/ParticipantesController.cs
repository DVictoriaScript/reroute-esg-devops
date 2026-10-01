using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReRoute.Api.DTOs;
using ReRoute.Api.Services;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Controllers;

/// <summary>
/// API REST de participantes do ReRoute (doadores, receptores, transportadores).
/// </summary>
[ApiController]
[Route("api/participantes")]
[Produces("application/json")]
public class ParticipantesController : ControllerBase
{
    private readonly IParticipanteService _service;

    public ParticipantesController(IParticipanteService service) => _service = service;

    /// <summary>Lista participantes com paginação.</summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<ParticipanteResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ParticipanteResponseDto>>> Listar([FromQuery] PaginationQuery query)
        => Ok(await _service.ListarAsync(query));

    /// <summary>Obtém participante por identificador.</summary>
    [HttpGet("{id:long}")]
    [Authorize]
    [ProducesResponseType(typeof(ParticipanteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ParticipanteResponseDto>> ObterPorId(long id)
        => Ok(await _service.ObterPorIdAsync(id));

    /// <summary>Cria um novo participante.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ParticipanteResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ParticipanteResponseDto>> Criar([FromBody] ParticipanteCreateDto dto)
    {
        var created = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = created.ParticipantId }, created);
    }

    /// <summary>Atualiza participante existente.</summary>
    [HttpPut("{id:long}")]
    [Authorize]
    [ProducesResponseType(typeof(ParticipanteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ParticipanteResponseDto>> Atualizar(long id, [FromBody] ParticipanteUpdateDto dto)
        => Ok(await _service.AtualizarAsync(id, dto));

    /// <summary>Remove participante.</summary>
    [HttpDelete("{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Excluir(long id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}