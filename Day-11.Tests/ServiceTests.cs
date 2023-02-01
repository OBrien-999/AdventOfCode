namespace Day_11.Tests;

[TestClass]
public class ServiceTests
{
    [TestMethod]
    public void CalculateLevelOfMonkeyBusiness_ReturnsCorrectResultForTwentyRounds()
    {
        // arrange
        var inputFilePath = @"./sample-input.txt";
        var service = new Service(inputFilePath);
        var expectedLevelOfMonkeyBusiness = 10605;
        
        // act
        var result = service.CalculateLevelOfMonkeyBusiness(20);

        // assert
        Assert.AreEqual(expectedLevelOfMonkeyBusiness, result);
    }

}