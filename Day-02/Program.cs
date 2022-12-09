var totalScore = 0;
var sortedSums = new SortedSet<Int32>();

var winMap = new Dictionary<string, string>()
{
    {Move.Rock, Move.Paper},
    {Move.Paper, Move.Scissors},
    {Move.Scissors, Move.Rock},
};

var loseMap = new Dictionary<string, string>()
{
    {Move.Paper, Move.Rock},
    {Move.Scissors, Move.Paper},
    {Move.Rock, Move.Scissors},
};

foreach(string line in System.IO.File.ReadLines(@"./day-02-input.txt"))
{
    var round = line.Split(" ");
    var opponentMove = round[0];
    var strategy = round[1];
    var myMove = getMyMoveForStrategy(strategy, opponentMove);

    var pointsForMyShape = getScoreForSelectedShape(myMove);
    var pointsForMove = getScoreForMove(strategy);

    totalScore += (pointsForMyShape + pointsForMove);
}

Console.WriteLine(totalScore);


int getScoreForSelectedShape(string selectedShape)
{
    switch(selectedShape)
    {
        case Move.Rock:
            return 1;
        case Move.Paper:
            return 2;
        case Move.Scissors:
            return 3;
        default:
            throw new NotSupportedException(selectedShape);
    }
}

int getScoreForMove(string strategy)
{
    if(strategy == RoundResult.Draw)
        return 3;

    if(strategy == RoundResult.Win)
        return 6;

    return 0;
}

string getMyMoveForStrategy(string strategy, string opponentMove)
{
    if(strategy == RoundResult.Draw)
        return opponentMove;

    if(strategy == RoundResult.Win)
        return winMap[opponentMove];

    return loseMap[opponentMove];
}

record RoundResult() 
{
    public const string Lose = "X";
    public const string Draw = "Y";
    public const string Win = "Z";
};

record Move() 
{
    public const string Rock = "A";
    public const string Paper = "B";
    public const string Scissors = "C";
};
