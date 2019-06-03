﻿using System;
using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Core;
using Newtonsoft.Json;
using NJsonSchema;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class CustomConvertersFixtures
    {
        private static string[] arrayScopes = { "read", "write" };

        private static List<Model.Security> listScopes = new List<Model.Security>()
        {
            new Model.Security { Key = "key", Scopes = arrayScopes }
        };

        private static List<Model.Security> multipleListScopes = new List<Model.Security>()
        {
            new Model.Security { Key = "key", Scopes = arrayScopes },
            new Model.Security { Key = "key", Scopes = arrayScopes }
        };

        [Fact]
        public void Validate_types_available_array_converter()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();

            //Act
            bool shouldFalse = converter.CanConvert(arrayScopes[0].GetType());
            bool shouldTrue = converter.CanConvert(arrayScopes.GetType());

            //Assert
            Assert.True(shouldTrue);
            Assert.False(shouldFalse);
        }

        [Fact]
        public void Invoke_array_serializer()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(arrayScopes.GetType());
            string response = JsonConvert.SerializeObject(arrayScopes, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.NotEmpty(response);
            Assert.All(arrayScopes, item => response.Contains(item));
        }

        [Fact]
        public void Invoke_array_serializer_empty()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();
            const string NullJson = "null";

            //Act
            bool shouldTrue = converter.CanConvert(arrayScopes.GetType());
            string response = JsonConvert.SerializeObject(null, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.Contains(NullJson, response);
        }

        [Fact]
        public void Invoke_array_deserializer()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(arrayScopes.GetType());
            string response = JsonConvert.SerializeObject(arrayScopes, Formatting.Indented, converter);

            //Assert
            Assert.Throws<NotImplementedException>(() => JsonConvert.DeserializeObject<string[]>(response, converter));
        }

        [Fact]
        public void Validate_types_available_collection_converter()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldFalse = converter.CanConvert(listScopes[0].GetType());
            bool shouldTrue = converter.CanConvert(listScopes.GetType());

            //Assert
            Assert.True(shouldTrue);
            Assert.False(shouldFalse);
        }

        [Fact]
        public void Invoke_collection_serializer()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(listScopes.GetType());
            string response = JsonConvert.SerializeObject(listScopes, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.NotEmpty(response);
            Assert.All(listScopes, item => response.Contains(item.Key));
        }


        [Fact]
        public void Invoke_collection_serializer_empty()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();
            const string NullJson = "null";

            //Act
            bool shouldTrue = converter.CanConvert(listScopes.GetType());
            string response = JsonConvert.SerializeObject(null, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.Contains(NullJson, response);
        }


        [Fact]
        public void Invoke_collection_deserializer()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(listScopes.GetType());
            string response = JsonConvert.SerializeObject(listScopes, Formatting.Indented, converter);

            //Assert
            Assert.Throws<NotImplementedException>(() => JsonConvert.DeserializeObject<List<Model.Security>>(response, converter));
        }

        [Fact]
        public void Invoke_multiple_collection_serializer()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(multipleListScopes.GetType());
            string response = JsonConvert.SerializeObject(multipleListScopes, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.NotEmpty(response);
            Assert.All(listScopes, item => response.Contains(item.Key));
        }

        [Fact]
        public void Type_name_generator_generate()
        {
            //Arrange
            var generator = new TypeNameGenerator();
            var schema = new JsonSchema();
            string typeNameHint = "My hint";
            string[] reservedNames = new string[] { "Reserved", "Names" };

            //Act
            string response = generator.Generate(schema, typeNameHint, reservedNames);

            //Assert
            Assert.Equal(typeNameHint, response);
        }
    }
}