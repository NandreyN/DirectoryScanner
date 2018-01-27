using FluentScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class BackgroundDistributor : Registry
    {
        private class Job : IJob
        {
            public void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public BackgroundDistributor()
        {
            JobManager.AddJob(new Job(), (s) => s.ToRunNow());
        }
    }

}
