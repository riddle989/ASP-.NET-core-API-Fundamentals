using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    //[Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider 
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            // For demo purpose, FileId is omitted
            var pathToFile = "3098b-manual.pdf";

            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            // If no content type matched, We will set a default content type
            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            Console.WriteLine("The content type is - " + contentType);
            
            var bytes = System.IO.File.ReadAllBytes(pathToFile);

            // In this scenario we set the contenttype to "plain/text" for all the files forcefully,
            // which will results in not rendering porperly the file content
            //return File(bytes, "plain/text", Path.GetFileName(pathToFile));


            // Now the contenttype is set automatically
            // The method "File()" is defined in "ControllerBase"
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
