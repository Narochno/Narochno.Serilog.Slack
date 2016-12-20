﻿using Narochno.Slack;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System;

namespace Narochno.Serilog.Slack
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration Slack(this LoggerSinkConfiguration loggerConfiguration, SlackConfig slackConfig, LogEventLevel minimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (slackConfig == null)
            {
                throw new ArgumentNullException(nameof(slackConfig));
            }

            var slackClient = new SlackClient(slackConfig);
            var batchingSink = new SlackBatchingSink(slackClient);
            return loggerConfiguration.Sink(batchingSink, minimumLevel);
        }
    }
}
