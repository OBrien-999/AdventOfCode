using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

var numberOfKnots = args.Count() > 0 ? Int32.Parse(args[0]) : 2;

TestMoveHeadOneStep();
TestParseInputMotion();
TestGenerateRopeSegments();
TestSampleInputForOneSegment();
TestSampleInputForMultipleSegmentsLargeSampleInput();

Console.WriteLine(CalculateNumberOfPositions(System.IO.File.ReadAllLines(@"./day-09-input.txt"), numberOfKnots));

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

void TestGenerateRopeSegments()
{
    var numberOfKnots = 10;
    var head = new Head();
    Dictionary<int, Tail> ropeSegments = GenerateRopeSegments(head, numberOfKnots);

    Assert.AreEqual(numberOfKnots - 1, ropeSegments.Count());
    var lastTail = ropeSegments[numberOfKnots - 1];
    var secondToLastTail = ropeSegments[numberOfKnots - 2];
    Assert.AreSame(lastTail.Head, secondToLastTail);
}

void TestSampleInputForOneSegment()
{
    string[] inputMotions = System.IO.File.ReadAllLines(@"./day-09-sample-input.txt");
    var numberOfKnots = 2;
    var expectedPositions = 13;

    var totalPositions = CalculateNumberOfPositions(inputMotions, numberOfKnots);

    Assert.AreEqual(expectedPositions, totalPositions);
}

void TestSampleInputForMultipleSegmentsLargeSampleInput()
{
    string[] inputMotions = System.IO.File.ReadAllLines(@"./Day-09-part2-sample2-sample-input.txt");
    var numberOfKnots = 10;
    var expectedPositions = 36;

    var totalPositions = CalculateNumberOfPositions(inputMotions, numberOfKnots);

    Assert.AreEqual(expectedPositions, totalPositions);
}

int CalculateNumberOfPositions(string[] motions, int numberOfKnots)
{
    Head head = new();
    Dictionary<int, Tail> ropeSegments = GenerateRopeSegments(head, numberOfKnots);

    foreach(var motion in motions)
    {
        (string direction, int distance) = ParseMotion(motion);

        foreach(var step in Enumerable.Range(0, distance))
        {
            head.Move(direction);

            foreach (var index in Enumerable.Range(1, numberOfKnots - 1))
            {
                var tail = ropeSegments[index];
                tail.Move();
            }
        }
    }

    return ropeSegments[ropeSegments.Count()].VisitedPoints.Count();
}

Dictionary<int, Tail> GenerateRopeSegments(Head head, int numberOfKnots)
{
    Dictionary<int, Tail> ropeSegments = new Dictionary<int, Tail>();

    foreach (var index in Enumerable.Range(1, numberOfKnots - 1))
    {
        Knot headOfCurrentKnot = index == 1 ? (Knot) head : ropeSegments[index - 1];
        ropeSegments.Add(index, new Tail(headOfCurrentKnot));
    }

    return ropeSegments;
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

    protected int _prevX = 0;
    public int PrevX { get { return _prevX; } }
    protected int _prevY = 0;
    public int PrevY { get { return _prevY; } }

    public Knot(int x = 0, int y = 0)
    {
        _x = x;
        _y = y;
    }
}


public record class Head : Knot
{
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
    private readonly Knot _head;
    public Knot Head { get { return _head; } }

    public readonly HashSet<string> VisitedPoints;
    
    public Tail(Knot head, int x = 0, int y = 0) : base(x, y)
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
        var isAdjacent = distance <= Math.Sqrt(2);
        if (!isAdjacent)
        {
            _prevX = X;
            _prevY = Y;

            var onSameColumn = _x == _head.X;
            var onSameRow = _y == _head.Y;

            if (!onSameColumn)
            {
                if (!onSameRow) // different column, different row, move diagonally
                {
                    if (_x < _head.X)
                        ++_x;
                    else
                        --_x;

                    if (_y < _head.Y)
                        ++_y;
                    else
                        --_y;
                } else { // different column, same row so we move left or right to get adjacent
                    if (_x < _head.X)
                        ++_x;
                    else
                        --_x;
                }
            }
            else { // on the same column. different row so we move up or down to get adjacent
                if (_y < _head.Y)
                    ++_y;
                else
                    --_y;
            }


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

