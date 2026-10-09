var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Several legacy aliases resolve to the same HTTP method and path.
    // Swagger can publish only one operation for each method/path pair.
    options.ResolveConflictingActions(actions => actions.First());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MockExternalApi v1");
    c.RoutePrefix = "swagger";
});

// Health probe — lets RTPSWB confirm the mock is alive
app.MapGet("/health", () => Results.Ok(new { status = "healthy", utc = DateTime.UtcNow }))
   .WithTags("Health");

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "3001";
app.Run($"http://0.0.0.0:{port}");
