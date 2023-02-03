using System.Text.RegularExpressions;
using Day_11.Models;

namespace Day_11;

public class Service
{
    private readonly string _inputFile;

    public Service(string inputFile)
    {
        _inputFile = inputFile;
    }

    public int CalculateLevelOfMonkeyBusiness(int numberOfRounds)
    {
        var monkeys = GenerateMonkeyDictFromInputFile();

        // process worry levels more optimally than original 
        for (int i = 0; i < numberOfRounds; i++)
        {
            foreach (var entry in monkeys)
            {
                var monkey = entry.Value;

                while (monkey.HasWorryLevelsToProcess())
                {
                    var (worryLevel, recipientIndex) = monkey.ProcessWorryLevel();

                    var recipientMonkey = monkeys[recipientIndex];
                    recipientMonkey.AddWorryLevel(worryLevel);
                }
            }
        }

        var result = monkeys
            .Select(x => x.Value.GetInspectedItemCount())
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((x, y) => x * y);

        return result;
    }

    public Dictionary<int, Monkey> GenerateMonkeyDictFromInputFile()
    {
        var fileText = System.IO.File.ReadAllText(_inputFile);
        var inputMonkeyItems = fileText.Split(Environment.NewLine + Environment.NewLine);

        var monkeys = new Dictionary<int, Monkey>();
        foreach (var monkeyItem in inputMonkeyItems)
        {
            string pattern = @"^Monkey (\d+):\r?\n\s*Starting items: (.*)\r?\n\s*Operation: new = (.*)\r?\n\s*Test: divisible by (\d+)\r?\n\s*If true: throw to monkey (\d+)\r?\n\s*If false: throw to monkey (\d+)$";
            MatchCollection matches = Regex.Matches(monkeyItem, pattern);

            var key = Int32.Parse(matches[0].Groups[1].Value);
            var items = ParseStartingItems(matches[0].Groups[2].Value.Trim());
            var operation = matches[0].Groups[3].Value.Trim();
            var testDivisor = Int32.Parse(matches[0].Groups[4].Value);
            var testPassRecipient = Int32.Parse(matches[0].Groups[5].Value);
            var testFailRecipient = Int32.Parse(matches[0].Groups[6].Value);

            monkeys.Add(
                key,
                new Monkey(items, operation, testDivisor, testPassRecipient, testFailRecipient)
            );
        }

        return monkeys;
    }

    public List<long> ParseStartingItems(string itemInput)
    {
        return itemInput.Split(", ").Select(Int64.Parse).ToList();
    }
}