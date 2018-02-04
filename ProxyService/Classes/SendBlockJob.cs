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
            string jsonRepresentation = JsonConvert.SerializeObject(_toSend.First());
            var response = await _httpClient.PostAsync(_deliveryAddress,
                new StringContent(jsonRepresentation, Encoding.UTF8, "application/json"));
            // HttpResuestException
            if (response.IsSuccessStatusCode)
                using (var context = new FolderRecordContext())
                {
                    _toSend.First().WasSent = true;
                    context.Update(_toSend.First());
                    await context.SaveChangesAsync();
                }
        }
    }
}
