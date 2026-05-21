using Microsoft.Extensions.Options;
using Supabase;
using FileOptions = Supabase.Storage.FileOptions;
using SupabaseOptions = Infrastructure.Options.SupabaseOptions;

namespace Infrastructure.Services;

public class SupabaseStorageService(Client client, IOptions<SupabaseOptions> options) : IStorageService
{
    private readonly SupabaseOptions _options = options.Value;

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType,
        CancellationToken cancellationToken)
    {
        using MemoryStream memoryStream = new();
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        byte[] fileBytes = memoryStream.ToArray();

        string filePath = $"{Guid.NewGuid()}/{fileName}";

        await client.Storage
            .From(_options.BucketName)
            .Upload(fileBytes, filePath, new FileOptions { ContentType = contentType, Upsert = false });

        return client.Storage
            .From(_options.BucketName)
            .GetPublicUrl(filePath);
    }

    public async Task DeleteAsync(string fileUrl)
    {
        string prefix = $"/storage/v1/object/public/{_options.BucketName}/";

        string absolutePath = Uri.UnescapeDataString(new Uri(fileUrl).AbsolutePath);
        int index = absolutePath.IndexOf(prefix, StringComparison.Ordinal);
        string filePath = index >= 0
            ? absolutePath[(index + prefix.Length)..]
            : throw new ArgumentException($"Invalid file URL: {fileUrl}");

        await client.Storage
            .From(_options.BucketName)
            .Remove([filePath]);
    }
}
