using Azure.Messaging.ServiceBus;

tring conn = "<<connectionString>>";
string queueName = "<<Queue Name>>";//"az204-queue";

ServiceBusClient client;
ServiceBusProcessor processor;

client = new ServiceBusClient(conn);
processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Iniciando proceso de lectura ... espere un momento");
    Console.ReadKey();
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}

async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($" Recibido:\t{body}");

    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)  
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}
