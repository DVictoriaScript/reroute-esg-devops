using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReRoute.Api.DTOs;
using ReRoute.Api.Services;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Controllers;

/// <summary>
/// API REST de solicitações - receptor manifesta interesse em uma doação.
/// </summary>
[ApiController]
[Route("api/solicitacoes")]
[Produces("application/json")]
public class SolicitacoesController : ControllerBase
{
    private readonly ISolicitacaoService _service;

    public SolicitacoesController(ISolicitacaoService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SolicitacaoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SolicitacaoResponseDto>>> Listar([FromQuery] PaginationQuery query)
        => Ok(await _service.ListarAsync(query));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(SolicitacaoResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SolicitacaoResponseDto>> ObterPorId(long id)
        => Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "RECEPTOR")]
    [ProducesResponseType(typeof(SolicitacaoResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<SolicitacaoResponseDto>> Criar([FromBody] SolicitacaoCreateDto dto)
    {
        var created = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = created.SolicitacaoId }, created);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "RECEPTOR")]
    [ProducesResponseType(typeof(SolicitacaoResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SolicitacaoResponseDto>> Atualizar(long id, [FromBody] SolicitacaoUpdateDto dto)
        => Ok(await _service.AtualizarAsync(id, dto));

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "RECEPTOR")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Excluir(long id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}