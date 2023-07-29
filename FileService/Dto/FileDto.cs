namespace FileService.Dto
{
    public record FileDto(string OriginalURL, string LocalName, string FileExtension, int FileSize);
    
}
