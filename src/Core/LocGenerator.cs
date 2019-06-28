namespace Nancy.Metadata.OpenApi.Core
{
    public enum Loc
    {
        Path,
        Header,
        Query,
        Cookie
    }

    internal static class LocGenerator
    {
        internal static string GetLocByEnum(Loc loc) => loc.ToString().ToLowerInvariant();
    }
}
