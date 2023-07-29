using FileService.Dto;

namespace FileService.Services
{
    public interface IFileService
    {
        Task<List<FileDto>> GetFiles();
        Task DownloadAndSaveFileByUrl(string directory, string url);
        Task UploadIFormFileInDirectory(string directory, IFormFile file);
        string GetImageUrl(string directory, string fileName);
    }
}
