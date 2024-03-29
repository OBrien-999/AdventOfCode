﻿using System;
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

    if (startPairOne >= startPairTwo && startPairOne <= endPairTwo) // start of first range is contained in the second range
        totalContainedPairs += 1;
    else if (endPairOne >= startPairTwo && endPairOne <= endPairTwo) // end of first range is contained in the second range
        totalContainedPairs += 1;
    else if (startPairOne < startPairTwo) // second range might contain first range
        totalContainedPairs += DoesRangeContainOther(startPairOne, endPairOne, startPairTwo, endPairTwo) ? 1 : 0;
    else if (startPairTwo < startPairOne) // first range might contain second range
        totalContainedPairs += DoesRangeContainOther(startPairTwo, endPairTwo, startPairOne, endPairOne) ? 1 : 0;
    else
        totalContainedPairs += 1;
}

Console.Write(totalContainedPairs);

static bool DoesRangeContainOther(int widerRangeStart, int widerRangeEnd, int shorterRangeStart, int shorterRangeEnd)
{
    IEnumerable<int> widerRange = Enumerable.Range(widerRangeStart, widerRangeEnd - widerRangeStart + 1);
    return widerRange.Contains(shorterRangeStart) && widerRange.Contains(shorterRangeEnd);
}