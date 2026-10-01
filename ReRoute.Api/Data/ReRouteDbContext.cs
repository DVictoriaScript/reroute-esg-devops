using Microsoft.EntityFrameworkCore;
using ReRoute.Api.Models;

namespace ReRoute.Api.Data;

/// <summary>
/// Contexto do Entity Framework Core para o domínio ReRoute.
/// Camada de acesso a dados - equivalente ao JpaRepository do projeto Java.
/// </summary>
/// <remarks>
/// TODO (Paula - Banco):
/// Motivo: alinhar EF Core ao schema Oracle do grupo (scripts Flyway Java V1–V3).
/// Arquivo: ReRouteDbContext.cs - revisar nomes de tabelas/colunas e relacionamentos.
/// Resultado esperado: migrations compatíveis com ESG_PARTICIPANTE, ESG_DOACAO, etc. no Oracle.
/// </remarks>
public class ReRouteDbContext : DbContext
{
    public ReRouteDbContext(DbContextOptions<ReRouteDbContext> options) : base(options)
    {
    }

    public DbSet<Participante> Participantes => Set<Participante>();
    public DbSet<Doacao> Doacoes => Set<Doacao>();
    public DbSet<Solicitacao> Solicitacoes => Set<Solicitacao>();
    public DbSet<Entrega> Entregas => Set<Entrega>();
    public DbSet<RegistroESG> RegistrosESG => Set<RegistroESG>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Nomes de tabela alinhados ao Oracle para facilitar migração futura.
        modelBuilder.Entity<Participante>(entity =>
        {
            entity.ToTable("ESG_PARTICIPANTE");
            entity.HasKey(e => e.ParticipantId);
            entity.Property(e => e.ParticipantId)
                .HasColumnName("PARTICIPANTID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Senha).HasColumnName("SENHA").HasMaxLength(200).IsRequired();
            entity.Property(e => e.TipoPerfil).HasColumnName("TIPOPERFIL").HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CREATEDAT");
        });

        modelBuilder.Entity<Doacao>(entity =>
        {
            entity.ToTable("ESG_DOACAO");
            entity.HasKey(e => e.DonationId);
            entity.Property(e => e.DonationId)
                .HasColumnName("DONATIONID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.NomeItem).HasColumnName("NOMEITEM").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Tamanho).HasColumnName("TAMANHO").HasMaxLength(20).IsRequired();
            entity.Property(e => e.LocalRetirada).HasColumnName("LOCALRETIRADA").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasColumnName("CREATEDAT").IsRequired();
            entity.Property(e => e.ParticipantId).HasColumnName("PARTICIPANTID");
            entity.HasOne(e => e.Participante).WithMany(p => p.Doacoes).HasForeignKey(e => e.ParticipantId);
        });

        modelBuilder.Entity<Solicitacao>(entity =>
        {
            entity.ToTable("ESG_SOLICITACAO");
            entity.HasKey(e => e.SolicitacaoId);
            entity.Property(e => e.SolicitacaoId)
                .HasColumnName("SOLICITACAOID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DonationId).HasColumnName("DONATIONID");
            entity.Property(e => e.ParticipantId).HasColumnName("PARTICIPANTID");
            entity.Property(e => e.DataSolicitacao).HasColumnName("DATASOLICITACAO").IsRequired();
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
            entity.HasOne(e => e.Doacao).WithMany(d => d.Solicitacoes).HasForeignKey(e => e.DonationId);
            entity.HasOne(e => e.Participante).WithMany(p => p.Solicitacoes).HasForeignKey(e => e.ParticipantId);
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.ToTable("ESG_ENTREGA");
            entity.HasKey(e => e.EntregaId);
            entity.Property(e => e.EntregaId)
                .HasColumnName("ENTREGAID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.SolicitacaoId).HasColumnName("SOLICITACAOID");
            entity.Property(e => e.ParticipantId).HasColumnName("PARTICIPANTID");
            entity.Property(e => e.DataAceite).HasColumnName("DATAACEITE");
            entity.Property(e => e.DataEntrega).HasColumnName("DATAENTREGA");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
            entity.HasOne(e => e.Solicitacao).WithMany(s => s.Entregas).HasForeignKey(e => e.SolicitacaoId);
            entity.HasOne(e => e.Participante).WithMany(p => p.Entregas).HasForeignKey(e => e.ParticipantId);
        });

        modelBuilder.Entity<RegistroESG>(entity =>
        {
            entity.ToTable("ESG_REGISTRO");
            entity.HasKey(e => e.RegistroId);
            entity.Property(e => e.RegistroId)
                .HasColumnName("REGISTROID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DonationId).HasColumnName("DONATIONID");
            entity.Property(e => e.ResiduoEvitado).HasColumnName("RESIDUOEVITADO").HasPrecision(5, 2);
            entity.Property(e => e.Co2Evitado).HasColumnName("CO2EVITADO").HasPrecision(5, 2);
            entity.Property(e => e.DataRegistro).HasColumnName("DATAREGISTRO").IsRequired();
            entity.HasOne(e => e.Doacao).WithMany(d => d.RegistrosESG).HasForeignKey(e => e.DonationId);
            entity.HasIndex(e => e.DonationId).HasDatabaseName("ESG_REGISTRO_IDX");
        });
    }
}
