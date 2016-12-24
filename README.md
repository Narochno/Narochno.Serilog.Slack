# Narochno.Serilog.Slack [![Build status](https://ci.appveyor.com/api/projects/status/7nhcewmc7ku6lf41/branch/master?svg=true)](https://ci.appveyor.com/project/Narochno/narochno-serilog-slack/branch/master)
A batching Serilog sink for Slack, narochno. Each log message is sent as an attachment on the same message, with the log event properties rendered out as fields.

![Screenshot](screenshot.png)

## Example Usage
```csharp
var config = new SlackConfig { WebHookUrl = "your webhook url" };

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Slack(config, LogEventLevel.Information)
    .CreateLogger();

Log.Information("Testing!");
```