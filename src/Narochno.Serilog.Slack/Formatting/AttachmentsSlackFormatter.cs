using Narochno.Slack.Entities;
using Serilog.Events;
using System.Collections.Generic;

namespace Narochno.Serilog.Slack.Formatting
{
    /// <summary>
    /// Formats each log message as an attachment.
    /// </summary>
    public class AttachmentsSlackFormatter : ISlackFormatter
    {
        public Message CreateMessage(IEnumerable<LogEvent> events)
        {
            return new Message
            {
                Attachments = GetAttachments(events)
            };
        }

        protected IEnumerable<Attachment> GetAttachments(IEnumerable<LogEvent> events)
        {
            foreach (var ev in events)
            {
                yield return new Attachment
                {
                    Timestamp = ev.Timestamp.ToUnixTimeSeconds(),
                    Color = ev.Level.GetColor(),
                    Text = ev.RenderMessage(null),
                    Fallback = $"[{ev.Level}] {ev.RenderMessage(null)}",
                    Footer = $"{ev.Level.GetEmoji()} {ev.Level}"
                };
            }
        }
    }
}
