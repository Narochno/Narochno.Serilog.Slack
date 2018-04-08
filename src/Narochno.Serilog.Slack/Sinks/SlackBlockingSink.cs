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
        private readonly ISlackClient client;
        private readonly ISlackFormatter formatter;

        public SlackBlockingSink(ISlackClient slackClient, ISlackFormatter formatter)
        {
            this.client = slackClient;
            this.formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            try
            {
                client.IncomingWebHook(formatter.CreateMessage(new[] { logEvent })).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("Exception while emitting message from {0}: {1}", this, ex);
            }
        }
    }
}
