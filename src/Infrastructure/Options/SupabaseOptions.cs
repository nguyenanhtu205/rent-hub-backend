namespace Infrastructure.Options;

public class SupabaseOptions
{
    public const string SectionName = "Supabase";
    public required string Url { get; set; }
    public required string Key { get; set; }
    public required string BucketName { get; set; }
}
