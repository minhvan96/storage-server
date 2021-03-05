﻿//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.StaticFiles;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Net.Http.Headers;
//using System.IO;
//using System.Net;
//using System.Threading.Tasks;
//using Vnr.Storage.API.Infrastructure.Utilities;

//namespace Vnr.Storage.API.Features.MimeTypeTest
//{
//    [Route("api/[controller]")]

//    public class MimeTypeController : Controller
//    {
//        private static readonly FormOptions _defaultFormOptions = new FormOptions();
//        private readonly long _fileSizeLimit;
//        private readonly string[] _permittedExtensions = { ".txt" };

//        public MimeTypeController()
//        {
//            _fileSizeLimit = 204857600;
//        }

//        [HttpGet]
//        public string Get(string fileName)
//        {
//            var provider = new FileExtensionContentTypeProvider();
//            string contentType;
//            if (!provider.TryGetContentType(fileName, out contentType))
//            {
//                contentType = "application/octet-stream";
//            }
//            return contentType;
//        }

//        [HttpPost]
//        public async Task<IActionResult> TestRSA(IFormFile file)
//        {
//            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
//            {
//                ModelState.AddModelError("File",
//                    $"The request couldn't be processed (Error 1).");
//                // Log error

//                return BadRequest(ModelState);
//            }

//            var boundary = MultipartRequestHelper.GetBoundary(
//                MediaTypeHeaderValue.Parse(Request.ContentType), _defaultFormOptions.MultipartBoundaryLengthLimit);
//            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
//            var section = await reader.ReadNextSectionAsync();

//            while (section != null)
//            {
//                var hasContentDispositionHeader =
//                    ContentDispositionHeaderValue.TryParse(
//                        section.ContentDisposition, out var contentDisposition);

//                if (hasContentDispositionHeader)
//                {
//                    // This check assumes that there's a file
//                    // present without form data. If form data
//                    // is present, this method immediately fails
//                    // and returns the model error.
//                    if (!MultipartRequestHelper
//                        .HasFileContentDisposition(contentDisposition))
//                    {
//                        ModelState.AddModelError("File",
//                            $"The request couldn't be processed (Error 2).");
//                        // Log error

//                        return BadRequest(ModelState);
//                    }
//                    else
//                    {
//                        // Don't trust the file name sent by the client. To display
//                        // the file name, HTML-encode the value.
//                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
//                                contentDisposition.FileName.Value);
//                        var trustedFileNameForFileStorage = Path.GetRandomFileName();

//                        // **WARNING!**
//                        // In the following example, the file is saved without
//                        // scanning the file's contents. In most production
//                        // scenarios, an anti-virus/anti-malware scanner API
//                        // is used on the file before making the file available
//                        // for download or for use by other systems.
//                        // For more information, see the topic that accompanies
//                        // this sample.

//                        //var streamedFileContent = await FileHelpers.ProcessStreamedFile(
//                        //    section, contentDisposition,Microsoft.AspNetCore.Mvc.ModelBinderAttribute ModelState,
//                        //    _permittedExtensions, _fileSizeLimit);

//                        //if (!ModelState.IsValid)
//                        //{
//                        //    return BadRequest(ModelState);
//                        //}

//                        //using (var targetStream = System.IO.File.Create(
//                        //    Path.Combine(_targetFilePath, trustedFileNameForFileStorage)))
//                        //{
//                        //    await targetStream.WriteAsync(streamedFileContent);

//                        //    _logger.LogInformation(
//                        //        "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
//                        //        "'{TargetFilePath}' as {TrustedFileNameForFileStorage}",
//                        //        trustedFileNameForDisplay, _targetFilePath,
//                        //        trustedFileNameForFileStorage);
//                        //}
//                    }
//                }

//                // Drain any remaining section body that hasn't been consumed and
//                // read the headers for the next section.
//                section = await reader.ReadNextSectionAsync();
//            }
//            return Ok();
//        }
//    }
//}