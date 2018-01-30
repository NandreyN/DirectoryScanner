using FluentScheduler;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class BackgroundDistributor : IJob
    {
        private PoolItemContext _poolContext;
        private FolderRecordContext _folderContext;

        public BackgroundDistributor(PoolItemContext poolContext, FolderRecordContext folderContext)
        {
            _folderContext = folderContext;
            _poolContext = poolContext;
        }

        public async void Execute()
        {
            for (int i = 0; i < _folderContext.Folders.Count(); i++)
            {
                bool success = await TryProcessOneItem(_folderContext.Folders.ElementAt<FolderRecord>(i));
                if (success)
                { }
                else { }
            }
        }

        private async Task<bool> TryProcessOneItem(FolderRecord record)
        {
            string address = await ReserveAvailableScannerAsync();
            if (string.IsNullOrEmpty(address))
                return false;

            var cts = new CancellationTokenSource();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(20);
                    var response = await httpClient.PostAsync(address, null, cts.Token);
                    // Handle response then

                    //... And save context!!!!
                    return true;
                }
            }
            catch (WebException webEx)
            {

                return false;
            }
            catch (TaskCanceledException cancelEx)
            {
                if (cancelEx.CancellationToken == cts.Token)
                {
                    // called by caller
                }
                else
                {
                    // timeout and other stuff
                }
                return false;
            }
        }


        private async Task<string> ReserveAvailableScannerAsync()
        {
            var scanner = _poolContext.PoolItems.Where(x => !x.IsBusy).FirstOrDefault();
            if (scanner == null)
                return null;

            scanner.IsBusy = true;
            await _poolContext.SaveChangesAsync();
            return scanner.Address;
        }

        private bool isAddressValid(string address)
        {
            throw new NotImplementedException();
        }
    }
}
