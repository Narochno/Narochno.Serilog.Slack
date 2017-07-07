using Serilog.Sinks.PeriodicBatching;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;
using System;
using Narochno.Slack;
using Narochno.Serilog.Slack.Formatting;

namespace Narochno.Serilog.Slack
{
    public class SlackBatchingSink : PeriodicBatchingSink
    {
        private readonly ISlackClient slackClient;
        private readonly ISlackFormatter messageFormatter;

        public SlackBatchingSink(ISlackClient slackClient, ISlackFormatter messageFormatter)
            : base(25, TimeSpan.FromSeconds(1))
        {
            this.slackClient = slackClient;
            this.messageFormatter = messageFormatter;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            await slackClient.IncomingWebHook(messageFormatter.CreateMessage(events));
        }

        protected override void Dispose(bool disposing)
        {
            slackClient.Dispose();
            base.Dispose(disposing);
        }
    }
}
