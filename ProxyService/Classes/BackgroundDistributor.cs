using FluentScheduler;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ProxyService.Classes
{
    public class BackgroundDistributor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public BackgroundDistributor(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        private async Task<int> ReserveAvailableScannerAsync(PoolItemContext context)
        {
            var scanner = context.PoolItems.FirstOrDefault(x => !x.IsBusy);
            if (scanner == null)
                return -1;

            scanner.IsBusy = true;
            await context.SaveChangesAsync();
            return scanner.Id;
        }

        private async Task ReleaseScannerAsync(PoolItemContext context, int id)
        {
            var item = context.PoolItems.First(x => x.Id == id);
            item.IsBusy = false;
            await context.SaveChangesAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var foldersContext = new FolderRecordContext())
                {
                    using (var poolItemContext = new PoolItemContext())
                    {
                        int total = foldersContext.Folders.Count();
                        if (total <= 0)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(3));
                            continue;
                        }

                        int availableId = await ReserveAvailableScannerAsync(poolItemContext);
                        
                        if (availableId == -1)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            continue;
                        }
                        //string address = poolItemContext.PoolItems.First(x => x.Id == availableId).Address ?? "";
                        // stuff
                        await ReleaseScannerAsync(poolItemContext,availableId);
                    }
                }
            }

            await StopAsync(cancellationToken: stoppingToken);
        }
    }
}
