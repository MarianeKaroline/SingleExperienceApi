using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SingleExperience.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase    {

        [HttpPost]
        public async Task<bool> UploadImage()
        {
            var file = Request.Form.Files[0];
            var path = @"C:\Users\nani_\Desktop\SingleExperience\SE-frontend\src\assets\images";
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(path, fileName);

                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return true;
        }
    }
}
