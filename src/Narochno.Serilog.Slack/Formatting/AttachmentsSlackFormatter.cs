using Narochno.Slack.Entities;
using System.Collections.Generic;
using Serilog.Events;
using System.Text;
using Serilog.Parsing;
using System.Linq;

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
                    Color = GetColor(ev.Level),
                    MrkdwnIn = new[] { "fields" },
                    Fields = GetFields(ev),
                    Fallback = $"[{ev.Level}] {ev.RenderMessage(null)}",
                    Footer = $"{GetEmoji(ev.Level)} {ev.Level}"
                };
            }
        }

        protected string GetPropertyValue(LogEventPropertyValue value)
        {
            var scalar = value as ScalarValue;
            if (scalar?.Value != null)
            {
                return scalar.Value.ToString();
            }

            return value.ToString();
        }

        protected string GetMessage(LogEvent logEvent)
        {
            var messageBuilder = new StringBuilder();
            foreach (var messageToken in logEvent.MessageTemplate.Tokens)
            {
                var messagePropertyToken = messageToken as PropertyToken;
                if (messagePropertyToken != null)
                {
                    var property = logEvent.Properties[messagePropertyToken.PropertyName];
                    messageBuilder.AppendFormat("`{0}`", GetPropertyValue(property));
                }
                else
                {
                    messageBuilder.Append(messageToken.ToString());
                }
            }
            return messageBuilder.ToString();
        }

        protected IEnumerable<Field> GetFields(LogEvent logEvent)
        {
            yield return new Field { Value = GetMessage(logEvent) };

            var propertyTokens = logEvent.MessageTemplate.Tokens.OfType<PropertyToken>().Select(x => x.PropertyName);

            var contextProperties = logEvent.Properties.Where(p => !p.Key.All(char.IsNumber) && !propertyTokens.Contains(p.Key));
            foreach (var property in contextProperties)
            {
                yield return new Field
                {
                    Title = property.Key,
                    Value = GetPropertyValue(property.Value),
                    Short = true
                };
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
    }
}
