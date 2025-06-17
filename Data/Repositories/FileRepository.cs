using Microsoft.Extensions.Options;
using Music.Data.Repositories.Interfaces;
using Uploadcare;
using Uploadcare.Upload;

namespace Music.Data.Repositories;

public class FileRepository(IOptions<UploadcareKeys> options) : IFileRepository
{
    private const int PageSize = 4;

    private readonly UploadcareClient _uploadcareClient = new(options.Value.PublicKey, options.Value.PrivateKey);

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        var fileUploder = new FileUploader(_uploadcareClient);
        var result = await fileUploder.Upload(fileBytes, file.FileName);
        return result.OriginalFileUrl;
    }
}