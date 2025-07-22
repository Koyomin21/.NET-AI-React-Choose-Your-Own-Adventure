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

        BindOptions<ChatSettingsOptions>(services, configuraton, ChatSettingsOptions.SectionName);
        var ollamaOptions = (BindOptions<AiOptions>(services, configuraton, AiOptions.SectionName))
            .Ollama;

        services.AddHttpClient(ollamaOptions.ClientName, client =>
        {
            client.BaseAddress = new Uri(ollamaOptions.BaseAdress);
            client.Timeout = TimeSpan.FromSeconds(ollamaOptions.TimeoutInSeconds);
        });
    }

    public static void AddPersistance(this IServiceCollection services, IConfiguration configuraton)
    {
        BindOptions<PersistenceOptions>(services, configuraton, PersistenceOptions.SectionName);

        services.AddDbContext<ApplicationDbContext>();
    }

    private static T BindOptions<T>(this IServiceCollection services, IConfiguration configuration, string sectionName)
        where T : class
    {
        var section = configuration.GetSection(sectionName);
        if (section is null || !section.Exists())
            throw new InvalidOperationException($"Missing `{sectionName}` section in Configuration");

        services.Configure<T>(section);

        var options = section.Get<T>() ?? throw new InvalidOperationException($"Failed to get the `{sectionName}` option instance for option type: `{nameof(T)}`");

        return options;
    }
}