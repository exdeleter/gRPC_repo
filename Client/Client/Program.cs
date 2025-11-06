using Grpc.Net.Client;
using Client;

using Service;

// создаем канал для обмена сообщениями с сервером
// параметр - адрес сервера gRPC
// using var channel = GrpcChannel.ForAddress("http://localhost:5287");
// // создаем клиент
// var client = new Greeter.GreeterClient(channel);
// Console.Write("Введите имя: ");
// var name = Console.ReadLine();
// // обмениваемся сообщениями с сервером
// var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
// Console.WriteLine($"Ответ сервера: {reply.Message}");
// Console.ReadKey();


using var channel = GrpcChannel.ForAddress("http://localhost:5287");
// создаем клиент
var client = new Person.PersonClient(channel);

var rnd = new Random();
while (true)
{
    var num = rnd.Next(1, 100);
    // обмениваемся сообщениями с сервером
    var req = new PersonRequest
    {
        Name = num.ToString(),
        Age = num,
        IsMarried = num % 2 == 0
    };
    Console.WriteLine($"Отправка сообщения с {num}");
    
    var reply = await client.SayHelloAsync(req);
    Console.WriteLine($"Ответ сервера: {reply.Message}");
    await Task.Delay(TimeSpan.FromSeconds(5));
}