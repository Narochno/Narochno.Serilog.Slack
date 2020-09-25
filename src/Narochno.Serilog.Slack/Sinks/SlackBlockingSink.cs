using Narochno.Serilog.Slack.Formatting;
using Narochno.Slack;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System;

namespace Narochno.Serilog.Slack.Sinks
{
    public sealed class SlackBlockingSink : ILogEventSink
    {
        private readonly ISlackClient _client;
        private readonly ISlackFormatter _formatter;

        public SlackBlockingSink(ISlackClient slackClient, ISlackFormatter formatter)
        {
            _client = slackClient;
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            try
            {
                _client.IncomingWebHook(_formatter.CreateMessage(new[] { logEvent })).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("Exception while emitting message from {0}: {1}", this, ex);
            }
        }
    }
}
