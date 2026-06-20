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
    }
}
