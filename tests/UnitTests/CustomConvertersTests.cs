using Nancy.Metadata.OpenApi.Core;
using System.Collections.Generic;
using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.UnitTests
{
    public class CustomConvertersTests
    {
        [Fact]
        public void Validate_types_available_array_converter()
        {
            //Arrange
            var converter = new CustomArrayJsonConverter();

            //Act
            bool shouldFalse = converter.CanConvert(typeof(int));
            bool shouldTrue = converter.CanConvert(typeof(int[]));

            //Assert
            Assert.True(shouldTrue);
            Assert.False(shouldFalse);
        }

        [Fact]
        public void Validate_types_available_collection_converter()
        {
            //Arrange
            var converter = new CustomCollectionJsonConverter();

            //Act
            bool shouldFalse = converter.CanConvert(typeof(int));
            bool shouldTrue = converter.CanConvert(typeof(List<int>));

            //Assert
            Assert.True(shouldTrue);
            Assert.False(shouldFalse);
        }
    }
}
