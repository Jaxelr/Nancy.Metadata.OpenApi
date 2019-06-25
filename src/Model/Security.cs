using System;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// No need to decorate this class for Json Props since this
    /// is calculated on the fly by the custom collection json converter
    /// </summary>
    public class Security : IEquatable<Security>
    {
        public string Key { get; set; }

        public string[] Scopes { get; set; }

        public bool Equals(Security other) => (Key == other.Key);
    }
}
