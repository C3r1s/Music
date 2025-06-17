namespace Music.Data.Repositories.Interfaces;

public interface IFileRepository
{
    Task<string> UploadFileAsync(IFormFile file);
}