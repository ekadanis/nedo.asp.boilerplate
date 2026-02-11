using Microsoft.EntityFrameworkCore;
using Nedo.Asp.Boilerplate.Application.Interfaces.Repositories;
using Nedo.Asp.Boilerplate.Application.Interfaces.Services;
using Nedo.Asp.Boilerplate.Application.Services;
using Nedo.Asp.Boilerplate.Infrastructure.Data;
using Nedo.Asp.Boilerplate.Infrastructure.Repositories;

namespace Nedo.Asp.Boilerplate.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IDocumentService, DocumentService>();

        return services;
    }
}
