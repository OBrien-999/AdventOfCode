using Day_12;

namespace Day12.Tests
{
    public class ServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetShortestPath_SampleInput_ReturnsSuccess()
        {
            //Arrange
            var service = new Service();

            //Act
            var result = service.GetShortestPath();

            //Assert
            Assert.That(result, Is.EqualTo(31));
        }
    }
}
