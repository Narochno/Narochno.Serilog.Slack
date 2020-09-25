# Narochno.Serilog.Slack [![Build status](https://ci.appveyor.com/api/projects/status/eju2eoyum80at14p/branch/master?svg=true)](https://ci.appveyor.com/project/Narochno/narochno-serilog-slack/branch/master) [![NuGet](https://img.shields.io/nuget/v/Narochno.Serilog.Slack.svg)](https://www.nuget.org/packages/Narochno.Serilog.Slack/)

A batching Serilog sink for Slack, narochno. Each log message is sent as an attachment on the same message, with the log event properties rendered out as fields.

![Screenshot](screenshot.png)

## Example Usage

```csharp
var webHookUrl = "your webhook URL";
var services = new ServiceCollection();
services.AddSlack(new SlackConfig()
{
    WebHookUrl = webHookUrl 
});
var serviceProvider = services.BuildServiceProvider();
var slackClient = serviceProvider.GetRequiredService<ISlackClient>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.SlackBlocking(slackClient, LogEventLevel.Information)
    .CreateLogger();

Log.Information("Testing!");
```

More on correct usage of DI and Serilog on `ASP.NET Core` can be found here.
https://nblumhardt.com/2020/09/serilog-inject-dependencies/

See the Narochno.Slack project for more information.
