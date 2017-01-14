using Narochno.Primitives;
using Serilog.Events;

namespace Narochno.Serilog.Slack.Formatting
{
    public static class LogEventLevelExtensions
    {
        public static Optional<string> GetEmoji(this LogEventLevel level)
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

        public static string GetColor(this LogEventLevel level)
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
