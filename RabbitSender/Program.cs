using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Sender App";

IConnection cnn = factory.CreateConnection();

IModel channel = cnn.CreateModel();

string ExchangeName = "Demo Exchange";
string routingKey = "demo-routing-key";
string queueName = "demo-queue-name";

channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, ExchangeName, routingKey, null);


for (int i = 0; i < 60; i++)
{
    Console.WriteLine($"sending message #{i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message : #{i}");
    channel.BasicPublish(ExchangeName,routingKey,null,messageBodyBytes);
    Thread.Sleep(1000);
}

channel.Close();
cnn.Close();