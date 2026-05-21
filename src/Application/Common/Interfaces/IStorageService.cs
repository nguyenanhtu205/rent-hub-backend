namespace Application.Common.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType,
        CancellationToken cancellationToken);

    Task DeleteAsync(string fileUrl);
}
