
var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();
    builder.Services.AddOpenApi(); // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddApplicationServices();
    builder.Services.AddPersistance(builder.Configuration);

}


var app = builder.Build();

{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    var environmentName = app.Environment.EnvironmentName;
    Console.WriteLine($"Current Environment: {environmentName}");

    if (environmentName != "Docker")
    {
        app.UseHttpsRedirection();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
