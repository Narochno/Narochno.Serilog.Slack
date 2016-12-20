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

        public SlackBatchingSink(ISlackClient slackClient) : base(50, TimeSpan.FromSeconds(5))
        {
            this.slackClient = slackClient;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var result = await slackClient.PostMessage(new Message
            {
                Attachments = GetAttachments(events).ToList()
            });
        }

        protected IEnumerable<Attachment> GetAttachments(IEnumerable<LogEvent> events)
        {
            foreach (var ev in events)
            {
                yield return new Attachment
                {
                    Text = ev.RenderMessage(),
                    Timestamp = ev.Timestamp.ToUnixTimeSeconds(),
                    Color = GetColor(ev.Level),
                    Fields = GetFields(ev.Properties).ToList(),
                    Footer = ev.Level.ToString()
                };
            }
        }

        protected IEnumerable<Field> GetFields(IReadOnlyDictionary<string, LogEventPropertyValue> properties)
        {
            foreach (var property in properties.Where(p => !p.Key.All(char.IsNumber)))
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
