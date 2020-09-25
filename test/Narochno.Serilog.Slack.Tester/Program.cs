using Narochno.Slack;
using Serilog;
using Serilog.Context;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace Narochno.Serilog.Slack.Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHookUrl = "your webhook URL";
            var services = new ServiceCollection();
            services.AddSlack(new SlackConfig()
            {
                WebHookUrl = webHookUrl 
            });
            var serviceProvider = services.BuildServiceProvider();
            var slackClient = serviceProvider.GetRequiredService<ISlackClient>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.SlackBatching(slackClient, batchSizeLimit:1, period: TimeSpan.FromSeconds(1))
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();

            Log.Information("I am some information");

            try
            {
                string test = null;
                test.Trim();
            }
            catch (Exception e)
            {
                Log.Error(e, "I am an exception");
            }

            AsyncException().Wait();

            using (LogContext.PushProperty("ProcessName", "Test.exe"))
            using (LogContext.PushProperty("ThreadId", 7))
            {
                Log.Information("I am some {Environment} information {Instance} hello {Region}", "Staging", 432432432, "eu-west-1");
            }

            Log.Warning("I am a warning");

            Console.ReadLine();
        }
        
        public static async Task AsyncException()
        {
            try
            {
                Console.WriteLine("test2");
                await AsyncException2();
            }
            catch (Exception e)
            {
                Log.Error(e, "I am an exception");
            }
        }

        public static async Task AsyncException2()
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync("only testing", 99);
            }
        }
    }
}
