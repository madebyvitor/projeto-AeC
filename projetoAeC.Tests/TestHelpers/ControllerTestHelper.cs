using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace projetoAeC.Tests.TestHelpers;

internal static class ControllerTestHelper
{
    public static void ConfigureAuthenticatedUser(Controller controller, int usuarioId)
    {
        var httpContext = new DefaultHttpContext
        {
            User = CreateUser(usuarioId)
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        controller.TempData = new TempDataDictionary(httpContext, new TestTempDataProvider());
    }

    private static ClaimsPrincipal CreateUser(int usuarioId)
    {
        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString())],
            authenticationType: "Test");

        return new ClaimsPrincipal(identity);
    }

    private sealed class TestTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object?> LoadTempData(HttpContext context)
        {
            return new Dictionary<string, object?>();
        }

        public void SaveTempData(HttpContext context, IDictionary<string, object?> values)
        {
        }
    }
}
