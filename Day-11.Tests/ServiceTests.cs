using AngouriMath;
using Day_11.Models;
using System.Text.RegularExpressions;

namespace Day_11.Tests;

[TestClass]
public class ServiceTests
{
    private const string InputFilePath = @".\sample-input.txt";

    private readonly Service _service;

    public ServiceTests()
    {
        _service = new Service(InputFilePath);
    }

    [TestMethod]
    public void CalculateLevelOfMonkeyBusiness_ReturnsCorrectResultForTwentyRounds()
    {
        // arrange
        var expectedLevelOfMonkeyBusiness = 10605;
        
        // act
        var result = _service.CalculateLevelOfMonkeyBusiness(20);

        // assert
        Assert.AreEqual(expectedLevelOfMonkeyBusiness, result);
    }

    [TestMethod]
    public void GenerateMonkeyDictFromInputFile_ReturnsCorrectResult()
    {
        // act
        var monkeys = _service.GenerateMonkeyDictFromInputFile();

        // assert
        Assert.AreEqual(4, monkeys.Count);

        var monkey = monkeys[0];
        Assert.AreEqual(2, monkey.Items.Count);
        Assert.AreEqual("old * 19", monkey.Operation);
        Assert.AreEqual(23, monkey.TestDivisor);
        Assert.AreEqual(2, monkey.TestPassRecipient);
        Assert.AreEqual(3, monkey.TestFailRecipient);

        monkey = monkeys[3];
        Assert.AreEqual(1, monkey.Items.Count);
        Assert.AreEqual("old + 3", monkey.Operation);
        Assert.AreEqual(17, monkey.TestDivisor);
        Assert.AreEqual(0, monkey.TestPassRecipient);
        Assert.AreEqual(1, monkey.TestFailRecipient);
    }

    [TestMethod]
    public void ParseStartingItems_ReturnsCorrectResult()
    {
        // arrange
        var itemInput = "79, 60, 97";

        // act
        var result = _service.ParseStartingItems(itemInput);

        // assert
        Assert.AreEqual(3, result.Count);
    }
}