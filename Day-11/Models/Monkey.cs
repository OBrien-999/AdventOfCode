using AngouriMath;

namespace Day_11.Models;

public class Monkey
{
    public const int ReliefFactor = 3;

    public readonly List<long> Items;
    public readonly string Operation;
    public readonly int TestDivisor;
    public readonly int TestPassRecipient;
    public readonly int TestFailRecipient;

    private int InspectedItemCount = 0;

    public Monkey(List<long> items, string operation, int testDivisor, int testPassRecipient, int testFailRecipient)
    {
        Items = items;
        Operation = operation;
        TestDivisor = testDivisor;
        TestPassRecipient = testPassRecipient;
        TestFailRecipient = testFailRecipient;
    }

    /// <summary>
    /// Returns the worry level of the first item in the list.
    /// </summary>
    public (long, int) ProcessWorryLevel()
    {
        long worryLevel = Items[0];
        Items.RemoveAt(0);

        var expr = (Entity)Operation.Replace("old", worryLevel.ToString());
        worryLevel = (long)expr.EvalNumerical();
        worryLevel /= ReliefFactor;

        var recipient = worryLevel % TestDivisor == 0
            ? TestPassRecipient
            : TestFailRecipient;

        InspectedItemCount++;

        return (worryLevel, recipient);
    }

    public void AddWorryLevel(long worryLevel)
    {
        Items.Add(worryLevel);
    }

    public bool HasWorryLevelsToProcess()
    {
        return Items.Any();
    }

    public int GetInspectedItemCount()
    {
        return InspectedItemCount;
    }
}