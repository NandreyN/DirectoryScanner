using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProxyService.Classes;
using ProxyService.Interfaces;
using Scanner1Service.Classes;
using ScannerService.Interfaces;

namespace ScannerService.Controllers
{
    [Route("api/[controller]")]
    public class ScannerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _accumulatorAddress;

        public ScannerController(HttpClient client, IConfiguration config)
        {
            _httpClient = client;
            _accumulatorAddress = config.GetSection("Addresses:Accumulator").Value
                                  ?? throw new ArgumentException("Accumulator address in not valid");
        }

        [HttpPost("Scan")]
        public async Task<JsonResult> Scan([FromBody]FolderRecord request)
        {
            if (request == null)
                return Json(new { IsSuccess = false, ErrorMessage = "Invalid request parameters" });
            if (string.IsNullOrEmpty(request.Path))
                return Json(new { IsSuccess = false, ErrorMessage = "Nothing to scan" });

            IFolderScanner scanner = new LocalFolderScanner();
            IEnumerable<IFile> files = scanner.GetFiles(new LocalFolder(request.Path));
            ICollection<IExtension> extensions = FilesToExtensionsAdapter.FilesToExtensions(files);

            //var response = await _httpClient.PostAsync(_accumulatorAddress, new StringContent(JsonConvert.SerializeObject(extensions), Encoding.UTF8));
            return Json(null);
        }
    }
}
