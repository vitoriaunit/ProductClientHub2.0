using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

// Configurar Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var todosApi = app.MapGroup("/todos");

// Endpoint para listar todos (vazio por enquanto)
todosApi.MapGet("/", () => Results.Ok(Array.Empty<Todo>()));

// Endpoint para buscar um todo por ID (retorna NotFound por padrão)
todosApi.MapGet("/{id}", (int id) => Results.NotFound());

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
