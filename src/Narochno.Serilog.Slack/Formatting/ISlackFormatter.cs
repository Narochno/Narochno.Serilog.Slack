using Narochno.Slack.Entities.Requests;
using Serilog.Events;
using System.Collections.Generic;

namespace Narochno.Serilog.Slack.Formatting
{
    public interface ISlackFormatter
    {
        IncomingWebHookRequest CreateMessage(IEnumerable<LogEvent> logEvents);
    }
}
