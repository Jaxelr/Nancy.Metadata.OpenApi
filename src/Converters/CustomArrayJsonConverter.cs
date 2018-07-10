using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Nancy.Metadata.OpenApi.Core
{
    public class CustomArrayJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType.IsArray;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jo = new JObject();

            if (value is string[] list)
            {
                foreach (string s in list)
                {
                    jo.Add(s, string.Empty);
                }

                jo.WriteTo(writer);
            }
        }
    }
}
