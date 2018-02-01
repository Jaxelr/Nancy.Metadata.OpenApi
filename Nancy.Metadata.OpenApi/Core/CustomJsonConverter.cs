using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Core
{
    public class CustomJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<string, NJsonSchema.JsonSchema4>);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var j = new JObject();

            // Rather crude hack to have all necessary type components on one level.
            // The good thing is that it's save: as we use Type.FullName as
            // schema type name, there shouldn't be any conflicts, if two requests have
            // same submodel of Namespace.SpecificType type, they are guaranteed to be the
            // same type
            foreach (var pair in (value as Dictionary<string, NJsonSchema.JsonSchema4>))
            {
                var el = JObject.Parse(pair.Value.ToJson().Replace("#/definitions/", "#/components/schemas/"));

                var defs = el.GetValue("definitions");

                if (defs != null)
                {
                    foreach (JProperty content in el.GetValue("definitions"))
                    {
                        j.Remove(content.Name);
                        j.Add(content.Name, content.Value);
                    }
                }

                el.Remove("definitions");
                el.Remove("$schema");

                j.Add(pair.Key, el);
            }

            j.WriteTo(writer);
        }
    }
}
