using FileService.Dto;
using FileService.Repository;

namespace FileService.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileProvider _fileProvider;
        public FileService(IFileRepository fileRepository, IFileProvider fileProvider)
        {
            _fileRepository = fileRepository;
            _fileProvider = fileProvider;
        }

        public async Task<List<FileDto>> GetFiles()
        {
            var files = await _fileRepository.GetAllFiles();
            return files.Select(a => PrepareFileDto(a)).ToList();
        }
        public async Task UploadIFormFileInDirectory(string directory, IFormFile file)
        {
            _fileProvider.CreateDirectoryIfNotExist(directory);
            var filePath = _fileProvider.GetFileDirctory(directory, file.FileName);
            await _fileProvider.UploadIFormFile(file, filePath);
        }
        public async Task DownloadAndSaveFileByUrl(string directory, string url)
        {
            var fileDto = DownloadAndUploadImageByUrl(directory, url);
            if (fileDto != null && !await ExistFile(fileDto.OriginalURL))
                await AddFile(fileDto);
        }
        public string? GetImageUrl(string directory, string fileName)=> _fileProvider.GetImageUrlByName(directory, fileName);

        private async Task AddFile(FileDto fileDto)
        {
            await _fileRepository.AddFile(new DAL.File
            {
                DownloadDate = DateTime.Now.ToUniversalTime(),
                FileExtension = fileDto.FileExtension,
                FileSize = fileDto.FileSize,
                LocalName = fileDto.LocalName,
                OriginalURL = fileDto.OriginalURL
            });
        }
        private FileDto? DownloadAndUploadImageByUrl(string directory, string imageUrl)
        {
            _fileProvider.CreateDirectoryIfNotExist(directory);
            var fileData = _fileProvider.DownloadImageByUrl(directory, imageUrl);
            if (fileData.FileDto != null)
                _fileProvider.UploadFileData(fileData, directory);
            return fileData.FileDto;
        }
        private FileDto PrepareFileDto(DAL.File file) => new FileDto(file.OriginalURL, file.LocalName, file.FileExtension, file.FileSize);
        private async Task<bool> ExistFile(string originalUrl) => await _fileRepository.Exist(originalUrl);

      
    }
}
