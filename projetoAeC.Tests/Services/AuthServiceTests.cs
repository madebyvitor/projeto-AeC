using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projetoAeC.Models;
using projetoAeC.Services;
using projetoAeC.Tests.TestHelpers;
using projetoAeC.ViewModels.Auth;

namespace projetoAeC.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task RegistrarAsync_DeveCriarUsuarioERetornarTrue_QuandoDadosForemValidos()
    {
        await using var context = TestDbContextFactory.Create();
        var service = new AuthService(context);
        var model = CriarRegisterViewModel();

        var result = await service.RegistrarAsync(model);

        result.Success.Should().BeTrue();
        result.ErrorMessage.Should().BeNull();

        var usuario = await context.Usuarios.SingleAsync();
        usuario.Nome.Should().Be("Usuário Teste");
        usuario.UsuarioNome.Should().Be("usuario.teste");
        usuario.SenhaHash.Should().NotBeNullOrWhiteSpace();
        usuario.SenhaHash.Should().NotBe(model.Senha);
    }

    [Fact]
    public async Task RegistrarAsync_DeveRetornarFalse_QuandoNomeDeUsuarioJaExistir()
    {
        await using var context = TestDbContextFactory.Create();
        var service = new AuthService(context);
        var model = CriarRegisterViewModel(usuario: "Usuario.Teste");
        await service.RegistrarAsync(model);

        var result = await service.RegistrarAsync(CriarRegisterViewModel(usuario: " usuario.teste "));

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Este usuário já está cadastrado.");
        context.Usuarios.Should().HaveCount(1);
    }

    [Fact]
    public async Task ValidarCredenciaisAsync_DeveRetornarUsuario_QuandoCredenciaisForemCorretas()
    {
        await using var context = TestDbContextFactory.Create();
        var usuario = new Usuario
        {
            Nome = "Usuário Teste",
            UsuarioNome = "usuario.teste"
        };
        usuario.SenhaHash = new PasswordHasher<Usuario>().HashPassword(usuario, "senha-segura");
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        var service = new AuthService(context);

        var usuarioRetornado = await service.ValidarCredenciaisAsync(new LoginViewModel
        {
            Usuario = " Usuario.Teste ",
            Senha = "senha-segura"
        });

        usuarioRetornado.Should().NotBeNull();
        usuarioRetornado!.Id.Should().Be(usuario.Id);
    }

    [Fact]
    public async Task ValidarCredenciaisAsync_DeveRetornarNull_QuandoSenhaForIncorreta()
    {
        await using var context = TestDbContextFactory.Create();
        var usuario = new Usuario
        {
            Nome = "Usuário Teste",
            UsuarioNome = "usuario.teste"
        };
        usuario.SenhaHash = new PasswordHasher<Usuario>().HashPassword(usuario, "senha-segura");
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        var service = new AuthService(context);

        var usuarioRetornado = await service.ValidarCredenciaisAsync(new LoginViewModel
        {
            Usuario = "usuario.teste",
            Senha = "senha-incorreta"
        });

        usuarioRetornado.Should().BeNull();
    }

    private static RegisterViewModel CriarRegisterViewModel(string usuario = "Usuario.Teste")
    {
        return new RegisterViewModel
        {
            Nome = " Usuário Teste ",
            Usuario = usuario,
            Senha = "senha-segura",
            ConfirmarSenha = "senha-segura"
        };
    }
}
