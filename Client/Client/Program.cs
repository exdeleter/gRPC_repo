using Client.Services;
using Client.Services.Abstractions;

using Grpc.Net.Client;

using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
// Регистрация gRPC клиента
builder.Services.AddSingleton(sp =>
{
    var channel = GrpcChannel.ForAddress("http://localhost:5287");
    return new Person.PersonClient(channel);
});
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/get", (IPersonService service) => service.Get());

app.Run();