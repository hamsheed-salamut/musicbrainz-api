using Core.Common.AppConfiguration;
using Core.Common.Persistence.Mssql;
using Infrastructure.Persistence.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMssql(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var mssqlDbOptions = new MssqlDbOptions();
            configuration.GetSection("MssqlDbOptions").Bind(mssqlDbOptions);
            serviceCollection.AddSingleton(mssqlDbOptions);

            var myssqlConnectionString = configuration.GetConnectionString("MssqlDbOptions");

            serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(myssqlConnectionString);
            });

            serviceCollection.AddTransient<IArtistRepository, ArtistRepository>();

            return serviceCollection;
        }
    }
}
