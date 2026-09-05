using MyWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Keep validation responses in our ApiResponse format instead of
        // ASP.NET Core's default validation payload.
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// One in-memory store is shared by all requests for the lifetime of the app.
builder.Services.AddSingleton<InMemoryDataStore>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<InMemoryDataStore>().Seed();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection is intentionally omitted while the local launch profile
// is HTTP-only. Re-enable it once an HTTPS endpoint is configured.
app.UseAuthorization();
app.MapControllers();

app.Run();
