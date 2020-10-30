using Autofac;
using Quartz;
using Quartz.Spi;

namespace Tristan.QuartzScheduler
{
    public class JobFactory : IJobFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public JobFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) =>
            (_lifetimeScope.Resolve(bundle.JobDetail.JobType) as IJob)!;

        public void ReturnJob(IJob job)
        {
        }
    }
}