using RouteService.Model;
using RouteService.Service;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires
builder.Services.AddControllers();
builder.Services.AddSingleton<IRouteService, RouteManager>();
builder.Services.AddEndpointsApiExplorer();

// Ajout de Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Route Service API", Version = "v1" });
});

// Configuration du CORS si nécessaire
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builderPolicy => builderPolicy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Génère Swagger JSON
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Route Service API v1");  // Lien vers le Swagger JSON
        options.RoutePrefix = string.Empty;  // Swagger UI sera accessible à la racine de l'application
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");  // Applique la politique CORS
app.UseAuthorization();
app.MapControllers();

app.Run();
