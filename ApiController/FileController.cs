using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace SagErpBlazor.ApiController
{
    [Route("api/File")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName, [FromQuery] string type)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "TicketsAttachments", fileName);

            if (System.IO.File.Exists(filePath))
            {

                var content = System.IO.File.ReadAllBytes(filePath);
               
                    return File(content, type);
                
            }
            else
            {
                return NotFound();
            }
        }
    }
}
