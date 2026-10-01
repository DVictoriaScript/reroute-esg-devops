using ReRoute.Api.Models;

namespace ReRoute.Api.Repositories;

public interface IParticipanteRepository : IRepository<Participante> { }

public class ParticipanteRepository : Repository<Participante>, IParticipanteRepository
{
    public ParticipanteRepository(Data.ReRouteDbContext context) : base(context) { }
}

public interface IDoacaoRepository : IRepository<Doacao> { }

public class DoacaoRepository : Repository<Doacao>, IDoacaoRepository
{
    public DoacaoRepository(Data.ReRouteDbContext context) : base(context) { }
}

public interface ISolicitacaoRepository : IRepository<Solicitacao> { }

public class SolicitacaoRepository : Repository<Solicitacao>, ISolicitacaoRepository
{
    public SolicitacaoRepository(Data.ReRouteDbContext context) : base(context) { }
}

public interface IEntregaRepository : IRepository<Entrega> { }

public class EntregaRepository : Repository<Entrega>, IEntregaRepository
{
    public EntregaRepository(Data.ReRouteDbContext context) : base(context) { }
}

public interface IRegistroESGRepository : IRepository<RegistroESG> { }

public class RegistroESGRepository : Repository<RegistroESG>, IRegistroESGRepository
{
    public RegistroESGRepository(Data.ReRouteDbContext context) : base(context) { }
}
