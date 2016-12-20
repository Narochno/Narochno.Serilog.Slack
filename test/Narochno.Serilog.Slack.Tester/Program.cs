using Narochno.Slack;
using Serilog;
using System;

namespace Narochno.Serilog.Slack.Tester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHookUrl = "your webhook URL";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Slack(new SlackConfig { WebHookUrl = webHookUrl })
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("I am some information");

            Log.Error(new Exception(), "I am an exception");

            Log.Information("I am some {Environment} information {Instance} hello {Region}", "Staging", 432432432, "eu-west-1");

            Log.Warning("I am a warning");

            Console.ReadLine();
        }
    }
}
