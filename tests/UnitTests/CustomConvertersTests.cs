using Nancy.Metadata.OpenApi.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.UnitTests
{
    public class CustomConvertersTests
    {
        private static string[] ArrayScopes = { "read", "write" };

        private static List<Model.Security> ListScopes = new List<Model.Security>()
        {
            new Model.Security { Key = "key", Scopes = ArrayScopes }
        };

        [Fact]
        public void Validate_types_available_array_converter()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();

            //Act
            bool shouldFalse = converter.CanConvert(ArrayScopes[0].GetType());
            bool shouldTrue = converter.CanConvert(ArrayScopes.GetType());

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
            bool shouldTrue = converter.CanConvert(ArrayScopes.GetType());
            string response = JsonConvert.SerializeObject(ArrayScopes, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.NotEmpty(response);
            Assert.All(ArrayScopes, item => response.Contains(item));
        }

        [Fact]
        public void Invoke_array_deserializer()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(ArrayScopes.GetType());
            string response = JsonConvert.SerializeObject(ArrayScopes, Formatting.Indented, converter);

            //Assert
            Assert.Throws<NotImplementedException>(() => JsonConvert.DeserializeObject<string[]>(response, converter));
        }

        [Fact]
        public void Validate_types_available_collection_converter()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldFalse = converter.CanConvert(ListScopes[0].GetType());
            bool shouldTrue = converter.CanConvert(ListScopes.GetType());

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
            bool shouldTrue = converter.CanConvert(ListScopes.GetType());
            string response = JsonConvert.SerializeObject(ListScopes, Formatting.Indented, converter);

            //Assert
            Assert.True(shouldTrue);
            Assert.NotEmpty(response);
            Assert.All(ListScopes, item => response.Contains(item.Key));
        }

        [Fact]
        public void Invoke_collection_deserializer()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldTrue = converter.CanConvert(ListScopes.GetType());
            string response = JsonConvert.SerializeObject(ListScopes, Formatting.Indented, converter);

            //Assert
            Assert.Throws<NotImplementedException>(() => JsonConvert.DeserializeObject<List<Model.Security>>(response, converter));
        }

    }
}
