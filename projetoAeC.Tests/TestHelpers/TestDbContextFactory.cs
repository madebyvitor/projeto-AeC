using Microsoft.EntityFrameworkCore;
using projetoAeC.Data;

namespace projetoAeC.Tests.TestHelpers;

internal static class TestDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}
