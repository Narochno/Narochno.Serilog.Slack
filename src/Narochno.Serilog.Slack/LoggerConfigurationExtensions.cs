using Narochno.Serilog.Slack.Formatting;
using Narochno.Serilog.Slack.Sinks;
using Narochno.Slack;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System;

namespace Narochno.Serilog.Slack
{
    public static class LoggerConfigurationExtensions
    {
        private static readonly TimeSpan DefaultBatchPeriod = TimeSpan.FromSeconds(5);
        private const int DefaultBatchSize = 25;

        /// <summary>
        /// Write to Slack using batches of messages in the background.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="slackClient">A ISlackClient object <see cref="SlackClient"/></param>
        /// <param name="logLevel">The minimum log level to invoke this sink for</param>
        /// <param name="formatter">A <see cref="ISlackFormatter"/> for Slack</param>
        /// <param name="batchSizeLimit">The maximum batch size for the sink to send</param>
        /// <param name="period">How often the sink should send messages to Slack</param>
        /// <returns></returns>
        public static LoggerConfiguration SlackBatching(this LoggerSinkConfiguration configuration, ISlackClient slackClient, LogEventLevel logLevel = LevelAlias.Minimum, ISlackFormatter formatter = null, int batchSizeLimit = DefaultBatchSize, TimeSpan period = default)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            slackClient = slackClient ?? throw new ArgumentNullException(nameof(slackClient));
            formatter = formatter ?? new FieldsSlackFormatter();
            period = period == default ? DefaultBatchPeriod : period;

            return configuration.Sink(new SlackBatchingSink(slackClient, formatter, batchSizeLimit, period), logLevel);
        }

        /// <summary>
        /// Write to Slack and block the application while the request completes.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="slackClient">A ISlackClient object <see cref="SlackClient"/></param>
        /// <param name="logLevel">The minimum log level to invoke this sink for</param>
        /// <param name="formatter">A <see cref="ISlackFormatter"/> for Slack</param>
        /// <returns></returns>
        public static LoggerConfiguration SlackBlocking(this LoggerSinkConfiguration configuration, ISlackClient slackClient, LogEventLevel logLevel = LevelAlias.Minimum, ISlackFormatter formatter = null)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            slackClient = slackClient ?? throw new ArgumentNullException(nameof(slackClient));
            formatter = formatter ?? new FieldsSlackFormatter();

            return configuration.Sink(new SlackBlockingSink(slackClient, formatter), logLevel);
        }
    }
}
