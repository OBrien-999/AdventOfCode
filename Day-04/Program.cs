using System;
using System.Text.RegularExpressions;

var totalContainedPairs = 0;

foreach(string sectionPair in System.IO.File.ReadLines(@"./day-04-input.txt"))
{
    string pattern = @"(\d+)-(\d+),(\d+)-(\d+)";
    MatchCollection matches = Regex.Matches(sectionPair, pattern);

    var startPairOne = Int32.Parse(matches[0].Groups[1].Value);
    var endPairOne = Int32.Parse(matches[0].Groups[2].Value);
    var startPairTwo = Int32.Parse(matches[0].Groups[3].Value);
    var endPairTwo = Int32.Parse(matches[0].Groups[4].Value);

    if(startPairOne < startPairTwo)
    {
        IEnumerable<int> widerRange = Enumerable.Range(startPairOne, endPairOne);
        var doesRangeContainOther = widerRange.Contains(startPairTwo) && widerRange.Contains(endPairTwo);
        totalContainedPairs += doesRangeContainOther ? 1 : 0;
    }
    else if(startPairTwo < startPairOne)
    {
        IEnumerable<int> widerRange = Enumerable.Range(startPairTwo, endPairTwo);
        var doesRangeContainOther = widerRange.Contains(startPairOne) && widerRange.Contains(endPairOne);
        totalContainedPairs += doesRangeContainOther ? 1 : 0;
    }
    else
    {
        totalContainedPairs += 1;
    }
}

Console.Write(totalContainedPairs);
