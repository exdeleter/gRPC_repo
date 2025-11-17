using Client.Services.Abstractions;

using Service;

namespace Client.Services;

public class PersonService(Person.PersonClient grpcClient) : IPersonService
{
    public async Task<string> Get()
    {
        var reply = await grpcClient.SayHelloAsync(new PersonRequest
        {
            Name = "API CALL",
            Age = 25,
            IsMarried = false
        });

        return reply.Message;
    }
}