namespace LoginPage.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> ReadFileContentAsync(string filePath);
        Task WriteToFileAsync(string filePath, string content);
    }
}
