namespace ForumAggregator.Infraestructure.DbContext;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

public class BloggingContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5000;Username=postgres;Password=postgres;Database=forum_aggregator");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        return new DatabaseContext(optionsBuilder.Options);
    }
}