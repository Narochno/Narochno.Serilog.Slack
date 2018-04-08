using Narochno.Slack;
using Serilog;
using Serilog.Context;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Narochno.Serilog.Slack.Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHookUrl = "your webhook URL";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.SlackBatching(new SlackConfig { WebHookUrl = webHookUrl })
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
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync("only testing", 99);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "I am an exception");
            }
        }
    }
}
