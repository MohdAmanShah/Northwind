using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; // To use SQL Server.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options; // To use IServiceCollection.

namespace Northwind.Context
{
    public static class NorthwindContextExtension
    {
        /// <summary>
        ///  Adds NorthwindDataContext to the specified IServiceCollection. Uses the 
        ///  Sql server database provider.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="serverName">NOA\\SQLEXPRESS</param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static IServiceCollection AddNorthwindDataContext(this IServiceCollection services, string databaseName = "Northwind", string serverName = "NOA\\SQLEXPRESS")
        {
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = serverName;
            builder.InitialCatalog = databaseName;
            builder.IntegratedSecurity = true;
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;
            builder.MultipleActiveResultSets = true;
            string connectionString = builder.ConnectionString;

            NorthwindContextLogger.WriteLine(connectionString);

            services.AddDbContext<NorthwindDataContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.LogTo(NorthwindContextLogger.WriteLine, new[] { Microsoft.EntityFrameworkCore
 .Diagnostics.RelationalEventId.CommandExecuting });
            },
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient
            );
            return services;
        }
    }
}
