using LoginPage.Application.Interfaces;

namespace LoginPage.Infrastructure.FileService
{
    public class FileService : IFileService
    {
        public async Task<string> ReadFileContentAsync(string filePath)
        {
            string data = await File.ReadAllTextAsync(filePath);

            // Revisit to correct the return data
            return "asdf";
        }

        public async Task WriteToFileAsync(string filePath, string content)
        {
            await File.AppendAllTextAsync(filePath, content);
        }
    }
}
