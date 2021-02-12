namespace Jiml.Extracting
{
    public interface IExtractor
    {
        Results.Extraction ExtractFrom(Context context);
    }
}