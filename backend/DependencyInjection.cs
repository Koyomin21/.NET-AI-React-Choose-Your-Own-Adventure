using Persistence;
using Services.Implementations;
using Services.Interfaces;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IStoryService, StoryService>();
    }

    public static void AddPersistance(this IServiceCollection services, IConfiguration configuraiton)
    {
        var persistenceSection = configuraiton.GetSection(PersistenceOptions.SectionName);
        if (!persistenceSection.Exists())
            throw new InvalidOperationException($"Missing `{PersistenceOptions.SectionName}` section in Configuration");

        services.Configure<PersistenceOptions>(persistenceSection);
        services.AddDbContext<ApplicationDbContext>();
    }
}