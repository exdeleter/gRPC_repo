

using Core;
using Core.Clients.Abstractions;
using Core.Services;

using Database;

using Grpc.Net.Client;

using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Регистрация gRPC клиента
builder.Services.AddSingleton(sp =>
{
    var channel = GrpcChannel.ForAddress("http://localhost:5287");
    return new DataProcessing.DataProcessingClient(channel);
});

builder.Services.AddScoped<IDataStreamingClient<ResponseItem>, GrpcDataStreaming>();

builder.Services.AddSingleton<IRequestQueue, RequestQueue>();

builder.Services.AddScoped<IRequestProcessor, RequestProcessor>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddHostedService<RequestProcessingWorker>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Development.json");
#else
    builder.Configuration.AddJsonFile("appsettings.json");
#endif

builder.Services.AddDatabase(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ApplyMigrate();

app.UseHttpsRedirection();

app.MapPost("/create", ([FromBody]RequestDto dto, IRequestService service) => service.CreateRequest(dto));

app.Run();