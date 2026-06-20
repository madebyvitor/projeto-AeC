using Microsoft.EntityFrameworkCore;
using projetoAeC.Models;

namespace projetoAeC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.HasKey(usuario => usuario.Id);

            entity.Property(usuario => usuario.Nome)
                .HasColumnName("nome")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(usuario => usuario.UsuarioNome)
                .HasColumnName("usuario")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(usuario => usuario.SenhaHash)
                .HasColumnName("senha")
                .IsRequired();

            entity.HasIndex(usuario => usuario.UsuarioNome)
                .IsUnique();
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.ToTable("Enderecos");

            entity.HasKey(endereco => endereco.Id);

            entity.Property(endereco => endereco.Cep)
                .HasColumnName("cep")
                .HasMaxLength(9)
                .IsRequired();

            entity.Property(endereco => endereco.Logradouro)
                .HasColumnName("logradouro")
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(endereco => endereco.Numero)
                .HasColumnName("numero")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(endereco => endereco.Complemento)
                .HasColumnName("complemento")
                .HasMaxLength(100);

            entity.Property(endereco => endereco.Bairro)
                .HasColumnName("bairro")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(endereco => endereco.Cidade)
                .HasColumnName("cidade")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(endereco => endereco.Uf)
                .HasColumnName("uf")
                .HasMaxLength(2)
                .IsRequired();

            entity.Property(endereco => endereco.Ibge)
                .HasColumnName("ibge")
                .HasMaxLength(20);

            entity.HasOne(endereco => endereco.Usuario)
                .WithMany(usuario => usuario.Enderecos)
                .HasForeignKey(endereco => endereco.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(endereco => endereco.UsuarioId);
        });
    }
}
