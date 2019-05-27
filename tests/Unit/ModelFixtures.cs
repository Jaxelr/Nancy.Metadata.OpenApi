using Xunit;

namespace Nancy.Metadata.OpenApi.Tests.Unit
{
    public class ModelFixtures
    {
        [Theory]
        [InlineData("key1")]
        public void Security_equality_true(string key)
        {
            //Arrange
            var security1 = new Model.Security() { Key = key };
            var security2 = new Model.Security() { Key = key };

            //Act
            bool result = security1.Equals(security2);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("key1", "key2")]
        public void Security_equality_false(string key1, string key2)
        {
            //Arrange
            var security1 = new Model.Security() { Key = key1 };
            var security2 = new Model.Security() { Key = key2 };

            //Act
            bool result = security1.Equals(security2);

            //Assert
            Assert.False(result);
        }
    }
}
