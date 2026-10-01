using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReRoute.Api.DTOs;
using ReRoute.Api.Services;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Controllers;

/// <summary>
/// API REST de registros ESG - impacto ambiental (resíduo evitado, CO₂).
/// </summary>
[ApiController]
[Route("api/registros")]
[Produces("application/json")]
[Authorize]
public class RegistrosESGController : ControllerBase
{
    private readonly IRegistroESGService _service;

    public RegistrosESGController(IRegistroESGService service) => _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<RegistroESGResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<RegistroESGResponseDto>>> Listar([FromQuery] PaginationQuery query)
        => Ok(await _service.ListarAsync(query));

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(RegistroESGResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RegistroESGResponseDto>> ObterPorId(long id)
        => Ok(await _service.ObterPorIdAsync(id));

    [HttpPost]
    [ProducesResponseType(typeof(RegistroESGResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<RegistroESGResponseDto>> Criar([FromBody] RegistroESGCreateDto dto)
    {
        var created = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = created.RegistroId }, created);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(RegistroESGResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RegistroESGResponseDto>> Atualizar(long id, [FromBody] RegistroESGUpdateDto dto)
        => Ok(await _service.AtualizarAsync(id, dto));

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Excluir(long id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}