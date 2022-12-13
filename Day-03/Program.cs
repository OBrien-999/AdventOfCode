var prioritiesSum = 0;

foreach(string pack in System.IO.File.ReadLines(@"./day-03-input.txt"))
{
    var startOfSecondCompartment = pack.Length / 2;
    var compartmentOne = pack.Substring(0, pack.Length / 2);
    var compartmentTwo = pack.Substring(startOfSecondCompartment);

    var compartmentItems = new HashSet<char>();
    foreach (char item in compartmentOne)
        compartmentItems.Add(item);

    foreach (char item in compartmentTwo)
    {
        if (compartmentItems.Contains(item)) {
            var value = GetItemValue(item);
            prioritiesSum += value;
            break;
        }

    }
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
