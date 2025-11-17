using System.Text.Json;

using Grpc.Core;

namespace Service.Services;

public class PersonService : Person.PersonBase
{
    private readonly ILogger<PersonService> _logger;

    public PersonService(ILogger<PersonService> logger)
    {
        _logger = logger;
    }

    public override Task<Response> SayHello(PersonRequest request, ServerCallContext context)
    {
        _logger.LogInformation(JsonSerializer.Serialize(request));
        return Task.FromResult(new Response
        {
            Message = "Hello " + request.Name
        });
    }

    string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };
    public override async Task ServerDataStream(Request request,
        IServerStreamWriter<ResponseStream> responseStream,
        ServerCallContext context)
    {
        foreach (var message in messages)
        {
            await responseStream.WriteAsync(new ResponseStream { Content = message });
            // для имитации работы делаем задержку в 1 секунду
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}