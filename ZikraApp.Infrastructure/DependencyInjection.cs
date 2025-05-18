using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZikraApp.Core.Interfaces;
using ZikraApp.Infrastructure.Data;
using ZikraApp.Infrastructure.UnitOfWork;

namespace ZikraApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IUnitOfWork, ZikraApp.Infrastructure.UnitOfWork.UnitOfWork>();

            return services;
        }
    }
} 