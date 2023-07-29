namespace FileService.Services
{
    public interface IFileService
    {
        Task AddFile(DAL.File file);
        Task<List<DAL.File>> GetAllFiles();
        Task<bool> Exist(string originalUrl);
    }
}
