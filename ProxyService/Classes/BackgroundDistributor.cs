using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;

namespace ProxyService.Classes
{
    public class BackgroundDistributor : IJob, IDisposable
    {
        private const int RestartInterval = 5;
        private const int BlockSize = 5;

        private readonly HttpClient _httpClient;
        public BackgroundDistributor()
        {
            _httpClient = new HttpClient();
        }

        public async void Execute()
        {
            int totalFolderCount;
            do
            {
                using (var folderContext = new FolderRecordContext())
                {
                    totalFolderCount = folderContext.Folders.Count();
                    if (totalFolderCount <= 0) continue;

                    var records =
                        folderContext.Folders.Where(x => !x.WasSent ||
                                                         string.IsNullOrEmpty(x.InnerToken)).Take(BlockSize).ToList();

                    if (!records.Any())
                        break; // All sent , but not deleted

                    (var scannerId, var address) = await ReserveScanner();
                    if (scannerId == -1)
                    {
                        await Task.Delay(RestartInterval);
                        continue;
                    }

                    foreach (var record in records)
                    {
                        if (string.IsNullOrEmpty(record.InnerToken))
                            record.InnerToken = JWTTokenProvider.ProvideInnerToken();
                    }

                    IJob sendJob = new SendBlockJob(records, address);
                    Schedule s = new Schedule(sendJob.Execute).AndThen(async () =>
                        await ReleaseScanner(scannerId));

                    JobManager.AddJob(sendJob, (sc) => s.Execute());

                    foreach (var record in records)
                        folderContext.Update(record);

                    await folderContext.SaveChangesAsync();
                }
            } while (totalFolderCount > 0);
        }

        private async Task<(int, string)> ReserveScanner()
        {
            using (var poolContext = new PoolItemContext())
            {
                var scanner = poolContext.PoolItems.FirstOrDefault(x => !x.IsBusy);
                if (scanner == null)
                {
                    return (-1, string.Empty);
                }

                scanner.IsBusy = true;
                try
                {
                    await poolContext.SaveChangesAsync();
                }
                catch (Exception e) when (e is DbUpdateConcurrencyException || e is DbUpdateException)
                {
                    return (-1, string.Empty);
                }

                return (scanner.Id, scanner.Address);
            }
        }

        private async Task<bool> ReleaseScanner(int id)
        {
            using (var poolContext = new PoolItemContext())
            {
                var poolItem = poolContext.PoolItems.SingleOrDefault(x => x.Id == id);
                if (poolItem == null)
                    throw new ArgumentNullException($"Pool set was corrupted: {id} not exists");
                poolItem.IsBusy = false;
                try
                {
                    await poolContext.SaveChangesAsync();
                }
                catch (Exception e) when (e is DbUpdateConcurrencyException || e is DbUpdateException)
                {
                    return false;
                }

                return true;
            }
        }

        public async void Dispose()
        {
            using (var poolContext = new PoolItemContext())
            {
                await poolContext.PoolItems.ForEachAsync(x => x.IsBusy = false);
                await poolContext.SaveChangesAsync();
            }
            _httpClient.Dispose();
            JobManager.AddJob<BackgroundDistributor>(x => x.ToRunOnceIn(RestartInterval).Seconds());
        }
    }
}
