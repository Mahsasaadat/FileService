namespace FileService.Repository
{
    public interface IFileRepository
    {
        Task AddFile(DAL.File file);
        Task<List<DAL.File>> GetAllFiles();
        Task<bool> Exist(string originalUrl);
    }
}
