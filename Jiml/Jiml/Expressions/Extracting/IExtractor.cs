namespace Jiml.Expressions.Extracting
{
    public interface IExtractor
    {
        Results.Extraction ExtractFrom(Context context);
    }
}