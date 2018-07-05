using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Core
{
    public class CustomJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IEnumerable).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var j = new JArray();

            if (value is IList<Model.Security> list)
            {
                var jo = new JObject();

                foreach (var t in list)
                {
                    var temp = JToken.FromObject(t.Scopes);
                    jo.Add(t.Key, temp);
                    j.Add(jo);
                }

                j.WriteTo(writer);
            }
        }
    }
}
