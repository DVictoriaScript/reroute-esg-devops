using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReRoute.Api.DTOs;
using ReRoute.Api.Services;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Controllers;

/// <summary>
/// API REST de entregas - transportador executa a logística da doação.
/// </summary>
[ApiController]
[Route("api/entregas")]
[Produces("application/json")]
public class EntregasController : ControllerBase
{
    private readonly IEntregaService _service;

    public EntregasController(IEntregaService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<EntregaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<EntregaResponseDto>>> Listar([FromQuery] PaginationQuery query)
        => Ok(await _service.ListarAsync(query));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EntregaResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<EntregaResponseDto>> ObterPorId(long id)
        => Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "TRANSPORTADOR")]
    [ProducesResponseType(typeof(EntregaResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<EntregaResponseDto>> Criar([FromBody] EntregaCreateDto dto)
    {
        var created = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = created.EntregaId }, created);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "TRANSPORTADOR")]
    [ProducesResponseType(typeof(EntregaResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<EntregaResponseDto>> Atualizar(long id, [FromBody] EntregaUpdateDto dto)
        => Ok(await _service.AtualizarAsync(id, dto));

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "TRANSPORTADOR")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Excluir(long id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}