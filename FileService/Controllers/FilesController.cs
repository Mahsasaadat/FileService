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
        private string _directory;
        private string _userDirectory;
        
        public FilesController(IConfiguration configuration, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _fileService = fileService;
            _directory = Path.Combine(webHostEnvironment.WebRootPath, configuration["ImageDirectory"]); ;
            _userDirectory = Path.Combine(_directory, configuration["UserImageDirectory"]);

        }
        [HttpGet]
        public async Task<ActionResult<List<FileDto>>> GetStoredFiles()
        {
            return Ok(await _fileService.GetFiles());
        }
        [HttpGet("DownloadByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DownloadFile(string fileName)
        {
            var bytes = _fileService.GetImageBytes(_directory, fileName);
            if (bytes==null)
                return BadRequest("File Not Found");
            return File(bytes, "image / jpeg");
        }
        [HttpPost("UploadByTextFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Upload(IFormFile textFile)
        {
            if(!ValidateFormat(textFile, "txt"))
                return BadRequest("File Format Not Supported");
            await _fileService.ReadFileLineAndDownload(textFile,_directory);
            return Ok();

        }
        [HttpPost("UploadUserImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadUserImage(IFormFile file)
        {
            if(!ValidateFormat(file, _configuration["SupportedImageFormats"]))
                return BadRequest("File Format Not Supported");
            await _fileService.UploadIFormFileInDirectory(_userDirectory, file);
            return Ok();

        }
        private bool ValidateFormat(IFormFile file, string supportedFormates) => supportedFormates.Contains(file.FileName.Split('.').Last().ToLower());
     

 
    }
}
