using System;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#security-requirement-object
    /// </summary>

    // No decoration to this class for Json Props since this is calc on the fly by the custom collection json converter
    public class Security : IEquatable<Security>
    {
        public string Key { get; set; }

        public string[] Scopes { get; set; }

        public bool Equals(Security other) => (Key == other.Key);
    }
}
