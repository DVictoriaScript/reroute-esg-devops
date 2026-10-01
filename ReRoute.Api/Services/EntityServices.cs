using ReRoute.Api.DTOs;
using ReRoute.Api.Mappings;
using ReRoute.Api.Middleware;
using ReRoute.Api.Repositories;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Services;

/// <summary>
/// Camada de regras de negócio - orquestra repositórios e DTOs.
/// Padrão acadêmico: Controller → Service → Repository → EF Core.
/// </summary>
public interface IParticipanteService
{
    Task<PagedResult<ParticipanteResponseDto>> ListarAsync(PaginationQuery query);
    Task<ParticipanteResponseDto> ObterPorIdAsync(long id);
    Task<ParticipanteResponseDto> CriarAsync(ParticipanteCreateDto dto);
    Task<ParticipanteResponseDto> AtualizarAsync(long id, ParticipanteUpdateDto dto);
    Task ExcluirAsync(long id);
}

public class ParticipanteService : IParticipanteService
{
    private readonly IParticipanteRepository _repository;

    public ParticipanteService(IParticipanteRepository repository) => _repository = repository;

    public async Task<PagedResult<ParticipanteResponseDto>> ListarAsync(PaginationQuery query)
    {
        var page = await _repository.GetPagedAsync(query.Page, query.PageSize);
        return new PagedResult<ParticipanteResponseDto>
        {
            Items = page.Items.Select(x => x.ToResponse()).ToList(),
            Page = page.Page,
            PageSize = page.PageSize,
            TotalItems = page.TotalItems,
            TotalPages = page.TotalPages
        };
    }

    public async Task<ParticipanteResponseDto> ObterPorIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Participante {id} não encontrado.");
        return entity.ToResponse();
    }

    public async Task<ParticipanteResponseDto> CriarAsync(ParticipanteCreateDto dto)
    {
        var created = await _repository.AddAsync(dto.ToEntity());
        return created.ToResponse();
    }

    public async Task<ParticipanteResponseDto> AtualizarAsync(long id, ParticipanteUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Participante {id} não encontrado.");
        entity.ApplyUpdate(dto);
        await _repository.UpdateAsync(entity);
        return entity.ToResponse();
    }

    public async Task ExcluirAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Participante {id} não encontrado.");
        await _repository.DeleteAsync(entity);
    }
}

public interface IDoacaoService
{
    Task<PagedResult<DoacaoResponseDto>> ListarAsync(PaginationQuery query);
    Task<DoacaoResponseDto> ObterPorIdAsync(long id);
    Task<DoacaoResponseDto> CriarAsync(DoacaoCreateDto dto);
    Task<DoacaoResponseDto> AtualizarAsync(long id, DoacaoUpdateDto dto);
    Task ExcluirAsync(long id);
}

public class DoacaoService : IDoacaoService
{
    private readonly IDoacaoRepository _repository;

    public DoacaoService(IDoacaoRepository repository) => _repository = repository;

    public async Task<PagedResult<DoacaoResponseDto>> ListarAsync(PaginationQuery query)
    {
        var page = await _repository.GetPagedAsync(query.Page, query.PageSize);
        return new PagedResult<DoacaoResponseDto>
        {
            Items = page.Items.Select(x => x.ToResponse()).ToList(),
            Page = page.Page,
            PageSize = page.PageSize,
            TotalItems = page.TotalItems,
            TotalPages = page.TotalPages
        };
    }

    public async Task<DoacaoResponseDto> ObterPorIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Doação {id} não encontrada.");
        return entity.ToResponse();
    }

    public async Task<DoacaoResponseDto> CriarAsync(DoacaoCreateDto dto)
    {
        var created = await _repository.AddAsync(dto.ToEntity());
        return created.ToResponse();
    }

    public async Task<DoacaoResponseDto> AtualizarAsync(long id, DoacaoUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Doação {id} não encontrada.");
        entity.ApplyUpdate(dto);
        await _repository.UpdateAsync(entity);
        return entity.ToResponse();
    }

    public async Task ExcluirAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Doação {id} não encontrada.");
        await _repository.DeleteAsync(entity);
    }
}

public interface ISolicitacaoService
{
    Task<PagedResult<SolicitacaoResponseDto>> ListarAsync(PaginationQuery query);
    Task<SolicitacaoResponseDto> ObterPorIdAsync(long id);
    Task<SolicitacaoResponseDto> CriarAsync(SolicitacaoCreateDto dto);
    Task<SolicitacaoResponseDto> AtualizarAsync(long id, SolicitacaoUpdateDto dto);
    Task ExcluirAsync(long id);
}

public class SolicitacaoService : ISolicitacaoService
{
    private readonly ISolicitacaoRepository _repository;

    public SolicitacaoService(ISolicitacaoRepository repository) => _repository = repository;

    public async Task<PagedResult<SolicitacaoResponseDto>> ListarAsync(PaginationQuery query)
    {
        var page = await _repository.GetPagedAsync(query.Page, query.PageSize);
        return new PagedResult<SolicitacaoResponseDto>
        {
            Items = page.Items.Select(x => x.ToResponse()).ToList(),
            Page = page.Page,
            PageSize = page.PageSize,
            TotalItems = page.TotalItems,
            TotalPages = page.TotalPages
        };
    }

    public async Task<SolicitacaoResponseDto> ObterPorIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Solicitação {id} não encontrada.");
        return entity.ToResponse();
    }

    public async Task<SolicitacaoResponseDto> CriarAsync(SolicitacaoCreateDto dto)
    {
        var created = await _repository.AddAsync(dto.ToEntity());
        return created.ToResponse();
    }

    public async Task<SolicitacaoResponseDto> AtualizarAsync(long id, SolicitacaoUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Solicitação {id} não encontrada.");
        entity.ApplyUpdate(dto);
        await _repository.UpdateAsync(entity);
        return entity.ToResponse();
    }

    public async Task ExcluirAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Solicitação {id} não encontrada.");
        await _repository.DeleteAsync(entity);
    }
}

public interface IEntregaService
{
    Task<PagedResult<EntregaResponseDto>> ListarAsync(PaginationQuery query);
    Task<EntregaResponseDto> ObterPorIdAsync(long id);
    Task<EntregaResponseDto> CriarAsync(EntregaCreateDto dto);
    Task<EntregaResponseDto> AtualizarAsync(long id, EntregaUpdateDto dto);
    Task ExcluirAsync(long id);
}

public class EntregaService : IEntregaService
{
    private readonly IEntregaRepository _repository;

    public EntregaService(IEntregaRepository repository) => _repository = repository;

    public async Task<PagedResult<EntregaResponseDto>> ListarAsync(PaginationQuery query)
    {
        var page = await _repository.GetPagedAsync(query.Page, query.PageSize);
        return new PagedResult<EntregaResponseDto>
        {
            Items = page.Items.Select(x => x.ToResponse()).ToList(),
            Page = page.Page,
            PageSize = page.PageSize,
            TotalItems = page.TotalItems,
            TotalPages = page.TotalPages
        };
    }

    public async Task<EntregaResponseDto> ObterPorIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Entrega {id} não encontrada.");
        return entity.ToResponse();
    }

    public async Task<EntregaResponseDto> CriarAsync(EntregaCreateDto dto)
    {
        var created = await _repository.AddAsync(dto.ToEntity());
        return created.ToResponse();
    }

    public async Task<EntregaResponseDto> AtualizarAsync(long id, EntregaUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Entrega {id} não encontrada.");
        entity.ApplyUpdate(dto);
        await _repository.UpdateAsync(entity);
        return entity.ToResponse();
    }

    public async Task ExcluirAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Entrega {id} não encontrada.");
        await _repository.DeleteAsync(entity);
    }
}

public interface IRegistroESGService
{
    Task<PagedResult<RegistroESGResponseDto>> ListarAsync(PaginationQuery query);
    Task<RegistroESGResponseDto> ObterPorIdAsync(long id);
    Task<RegistroESGResponseDto> CriarAsync(RegistroESGCreateDto dto);
    Task<RegistroESGResponseDto> AtualizarAsync(long id, RegistroESGUpdateDto dto);
    Task ExcluirAsync(long id);
}

public class RegistroESGService : IRegistroESGService
{
    private readonly IRegistroESGRepository _repository;

    public RegistroESGService(IRegistroESGRepository repository) => _repository = repository;

    public async Task<PagedResult<RegistroESGResponseDto>> ListarAsync(PaginationQuery query)
    {
        var page = await _repository.GetPagedAsync(query.Page, query.PageSize);
        return new PagedResult<RegistroESGResponseDto>
        {
            Items = page.Items.Select(x => x.ToResponse()).ToList(),
            Page = page.Page,
            PageSize = page.PageSize,
            TotalItems = page.TotalItems,
            TotalPages = page.TotalPages
        };
    }

    public async Task<RegistroESGResponseDto> ObterPorIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Registro ESG {id} não encontrado.");
        return entity.ToResponse();
    }

    public async Task<RegistroESGResponseDto> CriarAsync(RegistroESGCreateDto dto)
    {
        var created = await _repository.AddAsync(dto.ToEntity());
        return created.ToResponse();
    }

    public async Task<RegistroESGResponseDto> AtualizarAsync(long id, RegistroESGUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Registro ESG {id} não encontrado.");
        entity.ApplyUpdate(dto);
        await _repository.UpdateAsync(entity);
        return entity.ToResponse();
    }

    public async Task ExcluirAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Registro ESG {id} não encontrado.");
        await _repository.DeleteAsync(entity);
    }
}
