namespace Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;

public class DocumentDto
{
    public required string Name { get; set; }

    public required PropertyDocumentType Type { get; set; }

    public required string Url { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<PropertyDocument, DocumentDto>();
        }
    }
}
