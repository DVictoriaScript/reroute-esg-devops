using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReRoute.Api.DTOs;
using ReRoute.Api.Services;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Controllers;

/// <summary>
/// API REST de doações - início do fluxo ReRoute (Doação → Solicitação → Entrega → ESG).
/// </summary>
[ApiController]
[Route("api/doacoes")]
[Produces("application/json")]
public class DoacoesController : ControllerBase
{
    private readonly IDoacaoService _service;

    public DoacoesController(IDoacaoService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DoacaoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<DoacaoResponseDto>>> Listar([FromQuery] PaginationQuery query)
        => Ok(await _service.ListarAsync(query));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(DoacaoResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DoacaoResponseDto>> ObterPorId(long id)
        => Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "DOADOR")]
    [ProducesResponseType(typeof(DoacaoResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<DoacaoResponseDto>> Criar([FromBody] DoacaoCreateDto dto)
    {
        var created = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = created.DonationId }, created);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "DOADOR")]
    [ProducesResponseType(typeof(DoacaoResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DoacaoResponseDto>> Atualizar(long id, [FromBody] DoacaoUpdateDto dto)
        => Ok(await _service.AtualizarAsync(id, dto));

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "DOADOR")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Excluir(long id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}