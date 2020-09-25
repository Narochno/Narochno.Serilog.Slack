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
        private readonly ISlackClient _client;
        private readonly ISlackFormatter _formatter;

        public SlackBatchingSink(ISlackClient client, ISlackFormatter formatter, int batchSizeLimit, TimeSpan period)
            : base(batchSizeLimit, period)
        {
            _client = client;
            _formatter = formatter;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            await _client.IncomingWebHook(_formatter.CreateMessage(events));
        }
    }
}
