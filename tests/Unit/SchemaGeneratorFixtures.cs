using System;
using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Core;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class SchemaGeneratorFixtures
    {
        [Fact]
        public void IsCollectionArray()
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
        public void IsCollectionList()
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
        public void IsNotCollectionString()
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
        public void IsNotCollectionDate()
        {
            //Arrange
            DateTime recs = DateTime.Now;
            //Act
            (bool iscollection, Type type) = SchemaGenerator.GetElementCollection(recs.GetType());

            //Assert
            Assert.False(iscollection);
            Assert.Equal(type, recs.GetType());
        }
    }
}
