namespace FileService.DAL
{
    public class File
    {
        public int Id { get; set; }
        public string OriginalURL { get; set; }
        public string LocalName { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
        public DateTime DownloadDate { get; set; }
    }
}
