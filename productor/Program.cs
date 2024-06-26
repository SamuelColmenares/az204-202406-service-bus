using Azure.Messaging.ServiceBus;

string conn = "<<connectionString>>";
string queueName = "<<Queue Name>>";//"az204-queue";

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(conn);
sender = client.CreateSender(queueName);

using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <=3; i++)
{
    if(!messageBatch.TryAddMessage(new ServiceBusMessage($"Mensaje productor enviado {i}"))){
        throw new Exception($"Exception en mensaje {i} ha ocurrido.");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine("Se envió mensajes a la cola");
}
catch (Exception ex)
{
    Console.WriteLine($"Se produjo error enviando: {ex.Message}");
}
finally{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Finaliza..");
Console.ReadKey();
