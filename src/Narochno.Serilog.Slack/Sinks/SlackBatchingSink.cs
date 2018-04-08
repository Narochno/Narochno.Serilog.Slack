using Serilog.Sinks.PeriodicBatching;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;
using Narochno.Slack;
using Narochno.Serilog.Slack.Formatting;
using System;

namespace Narochno.Serilog.Slack.Sinks
{
    public sealed class SlackBatchingSink : PeriodicBatchingSink
    {
        private readonly ISlackClient client;
        private readonly ISlackFormatter formatter;

        public SlackBatchingSink(ISlackClient client, ISlackFormatter formatter, int batchSizeLimit, TimeSpan period)
            : base(batchSizeLimit, period)
        {
            this.client = client;
            this.formatter = formatter;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            await client.IncomingWebHook(formatter.CreateMessage(events));
        }
    }
}
