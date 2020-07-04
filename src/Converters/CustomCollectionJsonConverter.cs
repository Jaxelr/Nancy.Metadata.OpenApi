using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nancy.Metadata.OpenApi.Core
{
    public class CustomCollectionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IEnumerable<Model.Security>).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
#pragma warning disable RCS1079 // Throwing of new NotImplementedException.
            throw new NotImplementedException();
#pragma warning restore RCS1079 // Throwing of new NotImplementedException.

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var j = new JArray();

            if (value is IList<Model.Security> list)
            {
                foreach (var t in list)
                {
                    var jo = new JObject();

                    var temp = JToken.FromObject(t.Scopes);
                    jo.Add(t.Key, temp);
                    j.Add(jo);
                }

                j.WriteTo(writer);
            }
        }
    }
}
