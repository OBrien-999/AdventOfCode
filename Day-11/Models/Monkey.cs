using AngouriMath;

namespace Day_11.Models;

public class Monkey
{
    public const int ReliefFactor = 3;

    public readonly List<int> Items;
    public readonly string Operation;
    public readonly int TestDivisor;
    public readonly int TestPassRecipient;
    public readonly int TestFailRecipient;

    public Monkey(List<int> items, string operation, int testDivisor, int testPassRecipient, int testFailRecipient)
    {
        Items = items;
        Operation = operation;
        TestDivisor = testDivisor;
        TestPassRecipient = testPassRecipient;
        TestFailRecipient = testFailRecipient;
    }

    // Operation: calculate worry level
    // divide by three and rounded down to the nearest integer  (relief factor)
    // Test
        // throw

    // Entity expr = "2 / 5 + 6";
    // Console.WriteLine((double)expr.EvalNumerical());
}