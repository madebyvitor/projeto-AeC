using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetoAeC.Controllers;
using projetoAeC.Data;
using projetoAeC.Models;
using projetoAeC.Tests.TestHelpers;
using projetoAeC.ViewModels.Enderecos;

namespace projetoAeC.Tests.Controllers;

public class EnderecosControllerTests
{
    [Fact]
    public async Task Index_DeveRetornarViewResult_ComEnderecosApenasDoUsuarioLogado()
    {
        await using var context = TestDbContextFactory.Create();
        context.Enderecos.AddRange(
            CriarEndereco(usuarioId: 1, cidade: "São Paulo", logradouro: "Rua B"),
            CriarEndereco(usuarioId: 1, cidade: "Campinas", logradouro: "Rua A"),
            CriarEndereco(usuarioId: 2, cidade: "Santos", logradouro: "Rua C"));
        await context.SaveChangesAsync();
        var controller = CriarController(context, usuarioId: 1);

        var result = await controller.Index();

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<Endereco>>().Subject;
        model.Should().HaveCount(2);
        model.Should().OnlyContain(endereco => endereco.UsuarioId == 1);
        model.Select(endereco => endereco.Cidade).Should().Equal("Campinas", "São Paulo");
    }

    [Fact]
    public async Task Create_Post_DeveRedirecionarParaIndex_QuandoModeloForValido()
    {
        await using var context = TestDbContextFactory.Create();
        var controller = CriarController(context, usuarioId: 7);
        var model = CriarEnderecoViewModel();

        var result = await controller.Create(model);

        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be(nameof(EnderecosController.Index));

        var endereco = await context.Enderecos.SingleAsync();
        endereco.UsuarioId.Should().Be(7);
        endereco.Cep.Should().Be("01001-000");
        endereco.Logradouro.Should().Be("Praça da Sé");
        endereco.Numero.Should().Be("100");
        endereco.Bairro.Should().Be("Sé");
        endereco.Cidade.Should().Be("São Paulo");
        endereco.Uf.Should().Be("SP");
    }

    [Fact]
    public async Task Create_Post_DeveRetornarViewResult_QuandoModelStateForInvalido()
    {
        await using var context = TestDbContextFactory.Create();
        var controller = CriarController(context, usuarioId: 7);
        var model = CriarEnderecoViewModel();
        controller.ModelState.AddModelError(nameof(EnderecoViewModel.Cep), "Informe um CEP válido.");

        var result = await controller.Create(model);

        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeSameAs(model);
        context.Enderecos.Should().BeEmpty();
    }

    private static EnderecosController CriarController(ApplicationDbContext context, int usuarioId)
    {
        var controller = new EnderecosController(context);
        ControllerTestHelper.ConfigureAuthenticatedUser(controller, usuarioId);

        return controller;
    }

    private static Endereco CriarEndereco(int usuarioId, string cidade, string logradouro)
    {
        return new Endereco
        {
            UsuarioId = usuarioId,
            Cep = "01001-000",
            Logradouro = logradouro,
            Numero = "100",
            Bairro = "Centro",
            Cidade = cidade,
            Uf = "SP"
        };
    }

    private static EnderecoViewModel CriarEnderecoViewModel()
    {
        return new EnderecoViewModel
        {
            Cep = "01001-000",
            Logradouro = " Praça da Sé ",
            Numero = " 100 ",
            Bairro = " Sé ",
            Cidade = " São Paulo ",
            Uf = " sp "
        };
    }
}
