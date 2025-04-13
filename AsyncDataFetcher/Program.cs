var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Nur IHttpClientFactory
builder.Services.AddScoped<ApiServices>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();