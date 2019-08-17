using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    /// <summary>
    /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.2.md#callbackObject
    /// </summary>
    public class Callback
    {
        public IDictionary<string, Dictionary<string, Endpoint>> PathInfos { get; set; }
    }
}
