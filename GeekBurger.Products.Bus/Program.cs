using Microsoft.Azure.ServiceBus;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Products.Bus
{

	class Program
	{
		const string QueueConnectionString = "Endpoint=sb://geekburgeratividade1.servicebus.windows.net/;SharedAccessKeyName=ProductPolicy;SharedAccessKey=xMwpjXymBBbjujV1AEh7AXjsK7JD3enn9j6oGtgPL4A=";
		const string QueuePath = "ProductChanged";
		static IQueueClient _queueClient;

		static void Main(string[] args)
		{
			if (args.Length <= 0 || args[0] == "sender")
			{
				SendMessagesAsync().GetAwaiter().GetResult();
				Console.WriteLine("messages were sent");
			}
			else if (args[0] == "receiver")
			{
				ReceiveMessagesAsync().GetAwaiter().GetResult();
				Console.WriteLine("messages were received");
			}
			else
				Console.WriteLine("nothing to do");

			Console.ReadLine();
		}

		private static async Task SendMessagesAsync()
		{
			_queueClient = new QueueClient(QueueConnectionString, QueuePath);
			var messages = "Hi,Hello,Hey,How are you,Be Welcome"
				.Split(',')
				.Select(msg =>
				{
					Console.WriteLine($"Will send message: {msg}");
					return new Message(Encoding.UTF8.GetBytes(msg));
				})
						.ToList();
			await _queueClient.SendAsync(messages);
			await _queueClient.CloseAsync();
		}

		private static async Task ReceiveMessagesAsync()
		{
			_queueClient = new QueueClient(QueueConnectionString, QueuePath);
			_queueClient.RegisterMessageHandler(MessageHandler,
				new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
			Console.ReadLine();
			await _queueClient.CloseAsync();
		}

		private static Task ExceptionHandler(ExceptionReceivedEventArgs exceptionArgs)
		{
			Console.WriteLine($"Message handler encountered an exception {exceptionArgs.Exception}.");
			var context = exceptionArgs.ExceptionReceivedContext;
			Console.WriteLine($"Endpoint:{context.Endpoint}, Path:{context.EntityPath}, Action:{context.Action}");
			return Task.CompletedTask;
		}

		private static async Task MessageHandler(Message message, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Received message: { Encoding.UTF8.GetString(message.Body)}");
			await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
		}

	}
}
