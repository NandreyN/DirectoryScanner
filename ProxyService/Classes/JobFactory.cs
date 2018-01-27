using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class JobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public void ReturnJob(IJob job)
        {
            throw new NotImplementedException();
        }
    }
}
