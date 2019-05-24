using NJsonSchema;
using System;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Core
{
    public class TypeNameGenerator : ITypeNameGenerator
    {
        /// <summary>
        /// This property uses Full Name to avoid collisions inside various namespaces.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Generate(Type type) => type.FullName;

        /// <summary>
        /// Used on the Json Schema generation.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="typeNameHint"></param>
        /// <param name="reservedTypeNames"></param>
        /// <returns></returns>
        public string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames) => typeNameHint;
    }
}
