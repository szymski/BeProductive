using Microsoft.EntityFrameworkCore;

namespace BeProductive.Modules.Common.Persistence;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions options) : base(options)
    {
    }
}