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
        public async Task<JsonResult> Scan([FromBody]IEnumerable<FolderRecord> r)
        {
            if (r == null)
                return Json(new { IsSuccess = false, ErrorMessage = "Invalid request parameters" });

            IEnumerable<FolderRecord> requests = r.ToList();
            
            foreach (var request in requests)
            {
                if (string.IsNullOrEmpty(request.Path))
                    return Json(new { IsSuccess = false, ErrorMessage = "Nothing to scan" });
            }

            IFolderScanner scanner = new LocalFolderScanner();
            List<IFile> files = new List<IFile>();
            foreach (var request in requests)
            {
                IEnumerable<IFile> newFiles = scanner.GetFiles(new LocalFolder(request.Path));
                foreach (var newFile in newFiles)
                {
                    files.Add(newFile);
                }
            }
            
            ICollection<IExtension> extensions = FilesToExtensionsAdapter.FilesToExtensions(files);

            //var response = await _httpClient.PostAsync(_accumulatorAddress, new StringContent(JsonConvert.SerializeObject(extensions), Encoding.UTF8));
            return Json(null);
        }
    }
}
