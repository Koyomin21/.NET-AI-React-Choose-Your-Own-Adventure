using Options;
using Persistence;
using Services.Implementations;
using Services.Interfaces;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuraton)
    {
        services.AddScoped<IStoryService, StoryService>();
        services.AddScoped<IChatService, ChatService>();

        BindOptions<AiOptions>(services, configuraton, AiOptions.SectionName);
        BindOptions<ChatSettingsOptions>(services, configuraton, ChatSettingsOptions.SectionName);
    }

    public static void AddPersistance(this IServiceCollection services, IConfiguration configuraton)
    {
        BindOptions<PersistenceOptions>(services, configuraton, PersistenceOptions.SectionName);

        services.AddDbContext<ApplicationDbContext>();
    }

    private static void BindOptions<T>(this IServiceCollection services, IConfiguration configuration, string sectionName)
        where T : class
    {
        var section = configuration.GetSection(sectionName);
        if (!section.Exists())
            throw new InvalidOperationException($"Missing `{sectionName}` section in Configuration");

        services.Configure<T>(section);
    }
}