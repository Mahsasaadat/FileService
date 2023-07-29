using FileService.Dto;
using FileService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private string directory;
        private string userDirectory;
        public FilesController(IConfiguration configuration, IFileService fileService)
        {
            _configuration = configuration;
            _fileService = fileService;
            directory = configuration["ImageDirectory"];
            userDirectory = Path.Combine(directory, configuration["UserImageDirectory"]);
        }
        [HttpGet("Get")]
        public async Task<ActionResult<List<FileDto>>> GetStoredFiles()
        {
            return Ok(await _fileService.GetFiles());
        }
        [HttpGet("GetUrlByName")]
        public ActionResult<string?> LoadImage(string fileName)
        {
            return Ok(_fileService.GetImageUrl(directory, fileName));
        }

        [HttpPost("UploadByTextFile")]
        public async Task<ActionResult> Upload(IFormFile textFile)
        {
            ValidateFormat(textFile, "txt");
            using (var reader = new StreamReader(textFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var uri = reader.ReadLine();
                    if (ValidateUrl(uri))
                        await _fileService.DownloadAndSaveFileByUrl(directory, uri);
                }

            }
            return Ok();

        }
        [HttpPost("UploadUserImage")]
        public async Task<ActionResult> UploadUserImage(IFormFile file)
        {
            ValidateFormat(file, _configuration["SupportedImageFormats"]);
            await _fileService.UploadIFormFileInDirectory(userDirectory, file);
            return Ok();

        }
        private static bool ValidateUrl(string uri)
        {
            Uri url;
            return Uri.TryCreate(uri, UriKind.Absolute, out url);
        }
        private void ValidateFormat(IFormFile file, string supportedFormates)
        {
            if (!supportedFormates.Contains(file.FileName.Split('.').Last().ToLower()))
                throw new Exception("File Format Not Supported");
        }
    }
}
