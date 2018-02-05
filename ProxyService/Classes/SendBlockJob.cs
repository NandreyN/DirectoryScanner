using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Newtonsoft.Json;

namespace ProxyService.Classes
{
    public class SendBlockJob : IJob
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IEnumerable<FolderRecord> _toSend;
        private readonly string _deliveryAddress;

        public SendBlockJob(IEnumerable<FolderRecord> toSend, string where)
        {
            _toSend = toSend;
            _deliveryAddress = where;
        }

        public async void Execute()
        {
            string jsonRepresentation = JsonConvert.SerializeObject(_toSend);
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PostAsync(_deliveryAddress,
                    new StringContent(jsonRepresentation, Encoding.UTF8, "application/json"));
            }
            catch (HttpRequestException)
            {
                // exit without saving progress
                return;
            }

            if (response?.IsSuccessStatusCode == true)
                using (var context = new FolderRecordContext())
                {
                    foreach (var item in _toSend)
                    {
                        item.WasSent = true;
                        context.Update(item);
                    }
                    await context.SaveChangesAsync();
                }
        }
    }
}
