using Serilog.Sinks.PeriodicBatching;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Events;
using System;
using Narochno.Slack;
using Narochno.Slack.Entities;
using System.Linq;
using System.IO;

namespace Narochno.Serilog.Slack
{
    public class SlackBatchingSink : PeriodicBatchingSink
    {
        private readonly ISlackClient slackClient;

        public SlackBatchingSink(ISlackClient slackClient) : base(25, TimeSpan.FromSeconds(1))
        {
            this.slackClient = slackClient;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var result = await slackClient.PostAttachments(GetAttachments(events));
        }

        protected IEnumerable<Attachment> GetAttachments(IEnumerable<LogEvent> events)
        {
            foreach (var ev in events)
            {
                yield return new Attachment
                {
                    Timestamp = ev.Timestamp.ToUnixTimeSeconds(),
                    Color = GetColor(ev.Level),
                    MrkdwnIn = new[] { "fields" },
                    Fields = GetFields(ev),
                    Footer = $"{GetEmoji(ev.Level)} {ev.Level}"
                };
            }
        }

        protected IEnumerable<Field> GetFields(LogEvent logEvent)
        {
            yield return new Field { Value = logEvent.RenderMessage() };

            foreach (var property in logEvent.Properties.Where(p => !p.Key.All(char.IsNumber)))
            {
                using (var tw = new StringWriter())
                {
                    yield return new Field
                    {
                        Title = property.Key,
                        Value = property.Value.ToString(),
                        Short = true
                    };
                }
            }

            if (logEvent.Exception != null)
            {
                yield return new Field
                {
                    Title = logEvent.Exception.GetType().FullName,
                    Value = $"```{logEvent.Exception}```",
                    Short = false
                };
            }
        }

        protected string GetEmoji(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Error:
                    return ":exclamation:";
                case LogEventLevel.Warning:
                    return ":warning:";
                case LogEventLevel.Fatal:
                    return ":x:";
                default:
                    return null;
            }
        }

        protected string GetColor(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Error:
                    return "danger";
                case LogEventLevel.Warning:
                    return "warning";
                default:
                    return "good";
            }
        }

        protected override void Dispose(bool disposing)
        {
            slackClient.Dispose();
            base.Dispose(disposing);
        }
    }
}
