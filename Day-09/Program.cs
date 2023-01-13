using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

TestMoveHeadOneStep();
TestParseInputMotion();
//TestSampleInput();

void TestMoveHeadOneStep()
{
    var head = new Knot(5, 2);
    int expectedX= 5;
    int expectedY = 1;

     MoveHeadOneStep(Direction.Down, head);

    Assert.AreEqual(expectedX, head.X);
    Assert.AreEqual(expectedY, head.Y);
}

void TestParseInputMotion()
{
    string inputMotion = "R 2";
    string expectedDirection = "R";
    int expectedDistance = 2;

    (string direction, int distance) = ParseMotion(inputMotion);

    Assert.AreEqual(expectedDirection, direction);
    Assert.AreEqual(expectedDistance, distance);
}

void TestSampleInput()
{
    string[] inputMotions = System.IO.File.ReadAllLines(@"./day-09-sample-input.txt");
    var expectedPositions = 13;

    var totalPositions = CalculateNumberOfPositions(inputMotions);

    Assert.AreEqual(expectedPositions, totalPositions);
}

int CalculateNumberOfPositions(string[] motions)
{
    // Problem summary:
    // Process each motion in the input
        // First motion (R4 - Right 4 positions): 
        // Move head 1 step at a time until the motion is complete
        // After each step check the distance from the tail if distance is greater than 1
        // Move the tail 

    Knot tail = new();
    Knot head = new();

    foreach(var motion in motions)
    {
        (string direction, int distance) = ParseMotion(motion);

        foreach(var step in Enumerable.Range(0, distance))
        {
            
        }
    }

    // Solution summary:
    // Keep the coordinates in a knot class for the head and tail
    // Use coordinate mathematics to calculate the distance between the head and tail
    // Everytime we move the tail to a coordinate we'll add that to a hashset
    // Return the count of the hashset

    return 0;
}

void MoveHeadOneStep(string direction, Knot head)
{
    switch(direction)
    {
        case Direction.Up:
            ++head.Y;
            break;
        case Direction.Down:
            --head.Y;
            break;
        case Direction.Left:
            --head.X;
            break;
        case Direction.Right:
            ++head.X;
            break;
    }
}

void MoveTailRelativeToHead(string direction, Knot tail)
{
    
}

(string direction, int distance) ParseMotion(string inputMotion)
{
    string pattern = @"([U|D|L|R]) (\d+)";
    MatchCollection matches = Regex.Matches(inputMotion, pattern);

    var direction = matches[0].Groups[1].Value;
    var distance = Int32.Parse(matches[0].Groups[2].Value);

    return (direction, distance);
}

public record class Knot 
{
    public int X;
    public int Y;

    public Knot(int x = 0, int y = 0)
    {
        X = x;
        Y = y;
    }
}

record struct Direction() 
{
    public const string Up = "U";
    public const string Down = "D";
    public const string Left = "L";
    public const string Right = "R";
};

