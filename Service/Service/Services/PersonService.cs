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
}