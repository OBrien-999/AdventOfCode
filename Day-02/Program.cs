var totalScore = 0;
var sortedSums = new SortedSet<Int32>();

var myWinMap = new Dictionary<string, string>()
{
    {MyMove.Rock, OpponentMove.Scissors},
    {MyMove.Paper, OpponentMove.Rock},
    {MyMove.Scissors, OpponentMove.Paper}
};

var drawMap = new Dictionary<string, int>()
{
    {MyMove.Rock, 0},
    {MyMove.Paper, 1},
    {MyMove.Scissors, 2},
    {OpponentMove.Rock, 0},
    {OpponentMove.Paper, 1},
    {OpponentMove.Scissors, 2}
};

foreach(string line in System.IO.File.ReadLines(@"./day-02-input.txt"))
{
    var round = line.Split(" ");
    var opponentMove = round[0];
    var myMove = round[1];
    
    var pointsForMyShape = getScoreForSelectedShape(myMove);
    var pointsForMove = getScoreForMove(opponentMove, myMove);

    totalScore += (pointsForMyShape + pointsForMove);
    //Console.WriteLine($"{opponentMove} {myMove} {pointsForMyShape} {pointsForMove} {totalScore}");
}

Console.WriteLine(totalScore);


int getScoreForSelectedShape(string selectedShape)
{
    switch(selectedShape)
    {
        case MyMove.Rock:
            return 1;
        case MyMove.Paper:
            return 2;
        case MyMove.Scissors:
            return 3;
        default:
            throw new NotSupportedException(selectedShape);
    }
}

int getScoreForMove(string opponentMove, string myMove)
{
    if(drawMap[myMove] == drawMap[opponentMove])
        return 3;

    if(myWinMap[myMove] == opponentMove)
        return 6;

    return 0;
}

record MyMove() 
{
    public const string Rock = "X";
    public const string Paper = "Y";
    public const string Scissors = "Z";
};

record OpponentMove() 
{
    public const string Rock = "A";
    public const string Paper = "B";
    public const string Scissors = "C";
};
