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

// using Grpc.Net.Client;
// using Client;
//
// using Service;
//
// // создаем канал для обмена сообщениями с сервером
// // параметр - адрес сервера gRPC
// // using var channel = GrpcChannel.ForAddress("http://localhost:5287");
// // // создаем клиент
// // var client = new Greeter.GreeterClient(channel);
// // Console.Write("Введите имя: ");
// // var name = Console.ReadLine();
// // // обмениваемся сообщениями с сервером
// // var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
// // Console.WriteLine($"Ответ сервера: {reply.Message}");
// // Console.ReadKey();
//
//
// using var channel = GrpcChannel.ForAddress("http://localhost:5287");
// // создаем клиент
// var client = new Person.PersonClient(channel);
//
// // await NewFunction(client);
// await NewFunctionStream(client);
//
// async Task NewFunction(Person.PersonClient personClient)
// {
//     var rnd = new Random();
//     while (true)
//     {
//         var num = rnd.Next(1, 100);
//         // обмениваемся сообщениями с сервером
//         var req = new PersonRequest
//         {
//             Name = num.ToString(),
//             Age = num,
//             IsMarried = num % 2 == 0
//         };
//         Console.WriteLine($"Отправка сообщения с {num}");
//     
//         var reply = await personClient.SayHelloAsync(req);
//         Console.WriteLine($"Ответ сервера: {reply.Message}");
//         await Task.Delay(TimeSpan.FromSeconds(5));
//     }
// }
//
// async Task NewFunctionStream(Person.PersonClient personClient)
// {
//     // посылаем пустое сообщение и получаем набор сообщений
//     var serverData = client.ServerDataStream(new Request());
//  
//     // получаем поток сервера
//     var responseStream = serverData.ResponseStream;
//     // с помощью итераторов извлекаем каждое сообщение из потока
//     while (await responseStream.MoveNext(CancellationToken.None))
//     {
//         var response = responseStream.Current;
//         Console.WriteLine(response.Content);
//     }
// }
//
//