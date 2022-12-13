var numberOfLines = args.Count() > 0 ? Int32.Parse(args[0]) : 1;

var prioritiesSum = 0;

var currentNumberOfLines = 0;
IEnumerable<char> duplicate = new char[]{};
foreach(string pack in System.IO.File.ReadLines(@"./day-03-input.txt"))
{
    if (numberOfLines == 1)
    {
        var startOfSecondCompartment = pack.Length / 2;
        var compartmentOne = pack.Substring(0, pack.Length / 2);
        var compartmentTwo = pack.Substring(startOfSecondCompartment);
        duplicate = compartmentOne.Intersect(compartmentTwo);
        prioritiesSum += GetItemValue(duplicate.First());
        continue;
    }

    if (currentNumberOfLines == numberOfLines)
    {
        prioritiesSum += GetItemValue(duplicate.First());
        duplicate = new char[]{};
        currentNumberOfLines = 0;
    }
    
    duplicate = !duplicate.Any()
        ? pack
        : pack.Intersect(duplicate);
    
    currentNumberOfLines++;
}

if (numberOfLines > 1)
{
    prioritiesSum += GetItemValue(duplicate.First());
}

Console.WriteLine(prioritiesSum);

int GetItemValue(char item)
{
    const int uppperCaseZ = 90;
    const int lowerCaseA = 97;

    const int lowerCaseShift = 96;
    const int upperCaseShift = 38;
    if (item >= lowerCaseA)     //item is lower case
        return (int) item - lowerCaseShift;
    if (item <= uppperCaseZ)    // item is upper case
        return (int) item - upperCaseShift;

    throw new ArgumentException($"Unsupported character {item}");
}
