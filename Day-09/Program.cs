using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

TestMoveHeadOneStep();
TestParseInputMotion();
TestSampleInput();

Console.WriteLine(CalculateNumberOfPositions(System.IO.File.ReadAllLines(@"./day-09-input.txt")));

void TestMoveHeadOneStep()
{
    var head = new Head(5, 2);
    int expectedX= 5;
    int expectedY = 1;

     head.Move(Direction.Down);

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

    Head head = new();
    Tail tail = new(head);

    foreach(var motion in motions)
    {
        (string direction, int distance) = ParseMotion(motion);

        foreach(var step in Enumerable.Range(0, distance))
        {
            head.Move(direction);
            tail.Move();
        }
    }

    return tail.VisitedPoints.Count();
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
    protected int _x;
    public int X { get { return _x; } }
    protected int _y;
    public int Y { get { return _y; } }

    public Knot(int x = 0, int y = 0)
    {
        _x = x;
        _y = y;
    }
}

public record class Head : Knot
{
    private int _prevX = 0;
    public int PrevX { get { return _prevX; } }
    private int _prevY = 0;
    public int PrevY { get { return _prevY; } }

    public Head(int x = 0, int y = 0) : base(x, y) {}

    public void Move(string direction)
    {
        _prevX = X;
        _prevY = Y;

         switch(direction)
        {
            case Direction.Up:
                ++_y;
                break;
            case Direction.Down:
                --_y;
                break;
            case Direction.Left:
                --_x;
                break;
            case Direction.Right:
                ++_x;
                break;
        }
    }
}

public record class Tail : Knot
{
    private readonly Head _head;

    public readonly HashSet<string> VisitedPoints;
    
    public Tail(Head head, int x = 0, int y = 0) : base(x, y)
    {
        _head = head;
        VisitedPoints = new HashSet<string>()
        {
            "0, 0"
        };
    }

    public void Move()
    {
        double distance = Math.Sqrt(Math.Pow(_head.X - X, 2) + Math.Pow(_head.Y - Y, 2));

        if (distance > Math.Sqrt(2))
        {
            _x = _head.PrevX;
            _y = _head.PrevY;

            VisitedPoints.Add($"{X}, {Y}");
        }
    }
}

record struct Direction() 
{
    public const string Up = "U";
    public const string Down = "D";
    public const string Left = "L";
    public const string Right = "R";
};

