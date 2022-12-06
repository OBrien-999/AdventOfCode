var sum = 0;
var maxSum = 0;

foreach(string line in System.IO.File.ReadLines(@"./day-01-input.txt"))
{
    if(!String.IsNullOrEmpty(line))
    {
        sum += Int32.Parse(line);
        continue;
    }

    if(sum > maxSum)
        maxSum = sum;

    sum = 0;
}

if(sum > maxSum)
   maxSum = sum;

Console.WriteLine(maxSum);