using System;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Security : IEquatable<Security>
    {
        public string Key { get; set; }

        public string[] Scopes { get; set; }

        public bool Equals(Security other) => (Key == other.Key);
    }
}
