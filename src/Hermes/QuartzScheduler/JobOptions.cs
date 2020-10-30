using System;

namespace Tristan.QuartzScheduler
{
    public class JobOptions
    {
        public JobOptions(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get; }
        public string CronExpression { get; }
    }
}