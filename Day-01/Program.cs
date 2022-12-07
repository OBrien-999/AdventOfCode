var sum = 0;
var numOfSums = args.Count() > 0 ? Int32.Parse(args[0]) : 1;
var sortedSums = new SortedSet<Int32>();

foreach(string line in System.IO.File.ReadLines(@"./day-01-input.txt"))
{
    if(!String.IsNullOrEmpty(line))
    {
        sum += Int32.Parse(line);
        continue;
    }

    sortedSums.Add(sum);

    sum = 0;
}

sortedSums.Add(sum);

Console.WriteLine(sortedSums.Reverse().Take(numOfSums).Sum());