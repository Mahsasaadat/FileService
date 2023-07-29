using FileService.Dto;

namespace FileService.Services
{
    public interface IFileService
    {
        Task<List<FileDto>> GetFiles();
        Task UploadIFormFileInDirectory(string directory, IFormFile file);
        string GetImageUrl(string directory, string fileName);
        Task ReadFileLineAndDownload(IFormFile file, string directory);
    }
}
