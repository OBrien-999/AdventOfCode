using System.Text.RegularExpressions;

var moveMultipleCratesAtATime = args.Count() > 0 ?  true : false;

var processingRearrangements = false;

var supplies = new Supplies(moveMultipleCratesAtATime);

foreach(string input in System.IO.File.ReadLines(@"./day-05-input.txt"))
{
    if (processingRearrangements)
    {
        RearrangeSupplies(input, supplies);
        continue;
    }

    InitiateSupplies(input, supplies);

    processingRearrangements = string.IsNullOrEmpty(input) ? true : false;

}

Console.WriteLine(GetTopCrates(supplies));



void RearrangeSupplies(string input, Supplies supplies)
{
    string pattern = @"move (\d+) from (\d+) to (\d+)";
    MatchCollection matches = Regex.Matches(input, pattern);

    var numberOfCratesToMove = Int32.Parse(matches[0].Groups[1].Value);
    var sourceStackNumber = Int32.Parse(matches[0].Groups[2].Value);
    var destinationStackNumber = Int32.Parse(matches[0].Groups[3].Value);

    supplies.AddTop(numberOfCratesToMove, sourceStackNumber, destinationStackNumber);
    
}

void InitiateSupplies(string input, Supplies supplies)
{
    string fullCrate = @"\[(.)\]";
    string missingCrate = @"\s?(\s{3})\s?";
    string pattern = $"{fullCrate}|{missingCrate}";
    MatchCollection matches = Regex.Matches(input, pattern);
    foreach (var match in matches.Select((data, index) => (data, index)))
    {
        var crate = match.data.Groups[1].Value;
        if (string.IsNullOrEmpty(crate))
            continue;
        
        var stack = match.index + 1;
        supplies.AddBottom(stack, crate);
    }
}

string GetTopCrates(Supplies supplies)
{
    var topCrates = "";
    foreach (var stackNumber in Enumerable.Range(1, supplies.GetNumberOfStacks()))
    {
        topCrates += supplies.GetTopCrateForStack(stackNumber);
    }

    return topCrates;
}

public class Supplies
{

    private readonly Dictionary<int, LinkedList<string>> crateStacks;
    private readonly bool _moveMultipleCratesAtATime;

    public Supplies(bool moveMultipleCratesAtATime)
    {
        crateStacks = new Dictionary<int, LinkedList<string>>();
        _moveMultipleCratesAtATime = moveMultipleCratesAtATime;
    }
    
    public void AddBottom(int destinationStack, string crate)
    {
        if (crateStacks.TryGetValue(destinationStack, out var stack)) {
            stack.AddFirst(crate);
            return;
        }
        
        var newStack = new LinkedList<string>();
        newStack.AddFirst(crate);
        crateStacks.Add(destinationStack, newStack);
    }

    public void AddTop(int count, int sourceStackNumber, int destinationStackNumber)
    {
        var sourceStack = crateStacks.GetValueOrDefault(sourceStackNumber);
        var cratesToMove = sourceStack?.TakeLast(count);
        if (!crateStacks.TryGetValue(destinationStackNumber, out var destinationStack)) {
            destinationStack = new LinkedList<string>();
            crateStacks.Add(destinationStackNumber, destinationStack);
        }

        if (!_moveMultipleCratesAtATime)
            cratesToMove = cratesToMove.Reverse();

        foreach (string crateToMove in cratesToMove)
            destinationStack.AddLast(crateToMove);

        foreach (var _ in Enumerable.Range(0, count))
            sourceStack.RemoveLast();
    }

    public string GetTopCrateForStack(int stackNumber)
    {
        var stack = crateStacks.GetValueOrDefault(stackNumber);
        
        return stack?.Last();
    }

    public int GetNumberOfStacks()
    {
        return crateStacks.Count();
    }
}