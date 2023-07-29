﻿using FileService.Dto;

namespace FileService.Services
{
    public interface IFileProvider
    {
        (FileDto? fileDto, byte[]? fileData) DownloadImageByUrl(string dir, string imageUrl);

        Task UploadIFormFile(IFormFile file, string path);
        void CreateDirectoryIfNotExist(string dir);
        string? GetImageUrlByName(string directory, string fileName);
        string GetFileDirctory(string dir, string fileName);
        void UploadFileData(byte[] fileData, string fileName, string directory);
    }
}