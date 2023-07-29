using FileService.Dto;
using System.Net;
using System.Net.Http.Headers;

namespace FileService.Services
{
    public class FileProvider : IFileProvider
    {
        public FileData DownloadImageByUrl(string dir, string imageUrl)
        {
            FileDto fileDto = null;
            using (WebClient webClient = new WebClient())
            {
                byte[] fileData = webClient.DownloadData(imageUrl);
                string disposition = webClient.ResponseHeaders.Get("Content-Disposition");
                string fileName = GetFileName(imageUrl, disposition);
                int fileSize = int.Parse(webClient.ResponseHeaders.Get("Content-Length"));
                if (fileName != null)
                    fileDto = new FileDto(imageUrl, fileName, GetFileExtension(imageUrl), fileSize);
                return new FileData(fileDto, fileData);
            }
        }
        public void UploadFileData(FileData fileData, string directory)
        {
            string fileDirectory = GetFileDirctory(directory, fileData.FileDto.LocalName);
            if (CheckIfFileExist(fileDirectory) || fileData == null)
                return;
            System.IO.File.WriteAllBytes(fileDirectory, fileData.Data);
        }
        public async Task UploadIFormFile(IFormFile file, string path)
        {
            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }
        }
        public string? GetImageUrlByName(string directory, string fileName)
        {
            DirectoryInfo directoryToSearch = new DirectoryInfo(directory);
            FileInfo fileInDir = directoryToSearch.GetFiles(fileName).FirstOrDefault();
            return fileInDir?.FullName;
        }
        public void CreateDirectoryIfNotExist(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        public string GetFileDirctory(string dir, string fileName) => Path.Combine(dir, fileName);
        private static bool CheckIfFileExist(string dir) => File.Exists(dir);
        private static string GetFileName(string imageUrl, string disposition)
        {
            string fileName = null;
            if (!string.IsNullOrEmpty(disposition)
                   && ContentDispositionHeaderValue.TryParse(disposition, out var parsedDisposition)
                   && !string.IsNullOrEmpty(parsedDisposition.FileName))
            {
                fileName = parsedDisposition.FileName.Trim('"');
            }
            else
                fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
            return fileName;
        }
        private static string GetFileExtension(string imageUrl) => Path.GetExtension(imageUrl).Split('&').FirstOrDefault();


    }
}
