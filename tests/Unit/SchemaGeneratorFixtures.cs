using System;
using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Core;
using Nancy.Metadata.OpenApi.Tests.Fakes;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class SchemaGeneratorFixtures
    {
        [Fact]
        public void Get_element_collection_is_collection_array()
        {
            //Arrange
            string[] recs = new string[] { string.Empty, string.Empty };

            //Act
            (bool iscollection, Type type) = SchemaGenerator.GetElementCollection(recs.GetType());

            //Assert
            Assert.True(iscollection);
            Assert.Equal(type, recs.GetType().GetElementType());
        }

        [Fact]
        public void Get_element_collection_is_collection_list()
        {
            //Arrange
            var recs = new List<string> { string.Empty, string.Empty };
            //Act
            (bool iscollection, Type type) = SchemaGenerator.GetElementCollection(recs.GetType());

            //Assert
            Assert.True(iscollection);
            Assert.Equal(type, recs.GetType().GetGenericArguments()[0]);
        }

        [Fact]
        public void Get_element_collection_is_not_collection_string()
        {
            //Arrange
            string recs = string.Empty;
            //Act
            (bool iscollection, Type type) = SchemaGenerator.GetElementCollection(recs.GetType());

            //Assert
            Assert.False(iscollection);
            Assert.Equal(type, recs.GetType());
        }

        [Fact]
        public void Get_element_collection_is_not_collection_date()
        {
            //Arrange
            DateTime recs = DateTime.Now;

            //Act
            (bool iscollection, Type type) = SchemaGenerator.GetElementCollection(recs.GetType());

            //Assert
            Assert.False(iscollection);
            Assert.Equal(type, recs.GetType());
        }

        [Fact]
        public void Get_properties_by_type_with_child_poco_type()
        {
            //Arrange
            var fakeRequest = new FakeRequest();

            //Act
            var result = SchemaGenerator.GetPropertiesByType(fakeRequest.GetType());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey(nameof(fakeRequest.Name).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(fakeRequest.Loc).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(fakeRequest.Description).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(fakeRequest.Deprecated).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(fakeRequest.Format).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(fakeRequest.Required).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(fakeRequest.Type).ToLowerInvariant()));
        }

        [Fact]
        public void Get_properties_by_type_with_child_poco_no_props()
        {
            //Arrange
            var fakeRequest = new FakeServer();

            //Act
            var result = SchemaGenerator.GetPropertiesByType(fakeRequest.GetType());

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Get_properties_by_type_with_child_poco()
        {
            //Arrange
            var parent = new FakeParentModel();
            const string array = "array";

            //Act
            var result = SchemaGenerator.GetPropertiesByType(parent.GetType());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey(nameof(parent.Name).ToLowerInvariant()));
            Assert.True(result.ContainsKey(nameof(parent.Children).ToLowerInvariant()));
            Assert.Equal(array, result[nameof(parent.Children).ToLowerInvariant()].Type);
            Assert.True(SchemaCache.ComponentCache.ContainsKey(typeof(FakeChildModel).Name));
        }

        [Fact]
        public void Get_properties_by_type_type()
        {
            //Arrange
            var type = typeof(Type);

            //Act
            var result = SchemaGenerator.GetPropertiesByType(typeof(Type));

            //Assert
            Assert.NotNull(result);
            Assert.True(result.ContainsKey(nameof(type.Name)
                .ToLowerInvariant()));
        }
    }
}
