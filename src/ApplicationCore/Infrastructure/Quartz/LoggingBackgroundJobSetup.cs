﻿using Microsoft.Extensions.Options;
using Quartz;

namespace ApplicationCore.Infrastructure.Quartz
{
    internal class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(LoggingBackgroundJob));
            options
                .AddJob<LoggingBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever())
                        );
        }
    }
}
