using Narochno.Slack.Entities;
using Serilog.Events;
using System.Collections.Generic;

namespace Narochno.Serilog.Slack.Formatting
{
    public interface ISlackFormatter
    {
        Message CreateMessage(IEnumerable<LogEvent> logEvents);
    }
}
