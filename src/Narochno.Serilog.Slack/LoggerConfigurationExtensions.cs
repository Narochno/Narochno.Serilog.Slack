using Narochno.Serilog.Slack.Formatting;
using Narochno.Slack;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System;
using System.Net.Http;

namespace Narochno.Serilog.Slack
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration Slack(this LoggerSinkConfiguration loggerConfiguration, SlackConfig slackConfig, LogEventLevel minimumLevel = LevelAlias.Minimum, HttpClient httpClient = null)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (slackConfig == null)
            {
                throw new ArgumentNullException(nameof(slackConfig));
            }

            var slackClient = new SlackClient(httpClient ?? new HttpClient(), slackConfig);
            var messageFormatter = new FieldsSlackFormatter();
            var batchingSink = new SlackBatchingSink(slackClient, messageFormatter);
            return loggerConfiguration.Sink(batchingSink, minimumLevel);
        }

        public static LoggerConfiguration Slack(this LoggerSinkConfiguration loggerConfiguration, SlackConfig slackConfig, ISlackFormatter messageFormatter, LogEventLevel minimumLevel = LevelAlias.Minimum, HttpClient httpClient = null)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (slackConfig == null)
            {
                throw new ArgumentNullException(nameof(slackConfig));
            }

            if (messageFormatter == null)
            {
                throw new ArgumentNullException(nameof(messageFormatter));
            }

            var slackClient = new SlackClient(httpClient ?? new HttpClient(), slackConfig);
            var batchingSink = new SlackBatchingSink(slackClient, messageFormatter);
            return loggerConfiguration.Sink(batchingSink, minimumLevel);
        }
    }
}
