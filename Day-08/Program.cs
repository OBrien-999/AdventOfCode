

string[] inputRows = System.IO.File.ReadAllLines(@"./day-08-input.txt");
int MAX_X = inputRows[0].Length - 1;
int MAX_Y = inputRows.Length - 1;

var grid = generateGrid(inputRows);

findVisibleTrees(grid, startX: 0, startY: 0, endX: MAX_X, endY: MAX_Y);

var visibleTreeCount = grid.Select(x => x.Value).Where(t => t.IsVisible).Count();
var highestScenicScore = grid.Select(x => x.Value.ScenicScore).Max();

Console.WriteLine($"Visible trees from outside the grid: {visibleTreeCount}");
Console.WriteLine($"Highest scenic score: {highestScenicScore}");


Dictionary<string, Tree> generateGrid(string[] inputRows)
{
    var grid = new Dictionary<string, Tree>();
    var row = 0;
    var column = 0;
    foreach (var inputRow in inputRows)
    {
        column = 0;
        foreach(var number in inputRow)
        {
            var tree = new Tree(column, row, Int32.Parse(number.ToString())) {
                IsVisibleFromLeftEdge = column == 0,
                IsVisibleFromRightEdge = column == MAX_X,
                IsVisibleFromTopEdge = row == 0,
                IsVisibleFromBottomEdge = row == MAX_Y
            };
            grid.Add(tree.ToString(), tree);
            column++;
        }

        row++;
    }

    return grid;
}

void findVisibleTrees(Dictionary<string, Tree> grid, int startX, int startY, int endX, int endY)
{
    var height = endY - startY;
    if (height == 0) // We are down to a grid of 1 x Y (one row)
    {
        var width = endX - startX;
        if (width == 0) // We are down to a grid of 1 x 1
        {
            var currentTree = grid.GetValueOrDefault($"{startX},{startY}");
            if (!currentTree.IsVisibleFromLeftEdge)
                currentTree.IsVisibleFromLeftEdge = CheckTreeVisibility(currentTree, currentTree.X - 1, currentTree.Y, Direction.Left);
            
            if (!currentTree.IsVisibleFromRightEdge)
                currentTree.IsVisibleFromRightEdge = CheckTreeVisibility(currentTree, currentTree.X + 1, currentTree.Y, Direction.Right);
            
            if (!currentTree.IsVisibleFromTopEdge)
                currentTree.IsVisibleFromTopEdge = CheckTreeVisibility(currentTree, currentTree.X, currentTree.Y - 1, Direction.Up);
            
            if (!currentTree.IsVisibleFromBottomEdge)
                currentTree.IsVisibleFromBottomEdge = CheckTreeVisibility(currentTree, currentTree.X, currentTree.Y + 1, Direction.Down);

            return;
        }

        var gridMidPointX = startX + (width / 2); 

        findVisibleTrees(grid, startX, startY, gridMidPointX, endY);

        findVisibleTrees(grid, gridMidPointX + 1, startY, endX, endY);

    } else {

        var gridMidPointY =  startY + (height / 2);

        findVisibleTrees(grid, startX, startY, endX, gridMidPointY);

        findVisibleTrees(grid, 0, gridMidPointY + 1, endX, endY);
    }
}

bool CheckTreeVisibility(Tree rootTree, int nextX, int nextY, string direction)
{
    var neighborTree = grid.GetValueOrDefault($"{nextX},{nextY}");
    var rootLargerThanNextNeighbor = rootTree.Value > neighborTree.Value;
    var rootEqualToNextNeighbor = rootTree.Value == neighborTree.Value;

    if (!rootLargerThanNextNeighbor || rootEqualToNextNeighbor)
    {
        SetDistanceToLastVisibleTree(rootTree, neighborTree, direction);
        return false;
    }

    return CheckBaseCaseOrContinueSearching(rootTree, neighborTree, direction);
}

bool CheckBaseCaseOrContinueSearching(Tree rootTree, Tree neighborTree, string direction)
{
    switch(direction)
    {
        case Direction.Left:
            if (neighborTree.X == 0 || neighborTree.IsVisibleFromLeftEdge)
            { 
                rootTree.ViewingDistanceLeft = rootTree.X;
                return true;
            }

            return CheckTreeVisibility(rootTree, neighborTree.X - 1, rootTree.Y, Direction.Left);

        case Direction.Right:
            if (neighborTree.X == MAX_X || neighborTree.IsVisibleFromRightEdge)
            {
                rootTree.ViewingDistanceRight = MAX_X - rootTree.X;
                return true;
            }

            return CheckTreeVisibility(rootTree, neighborTree.X + 1, rootTree.Y, Direction.Right);

        case Direction.Up:
            if (neighborTree.Y == 0 || neighborTree.IsVisibleFromTopEdge)
            {
                rootTree.ViewingDistanceTop = rootTree.Y;
                return true;
            }

            return CheckTreeVisibility(rootTree, rootTree.X, neighborTree.Y - 1, Direction.Up);

        default:
            if (neighborTree.Y == MAX_Y || neighborTree.IsVisibleFromBottomEdge)
            {
                rootTree.ViewingDistanceBottom = MAX_Y - rootTree.Y;
                return true;
            }
                    
            return CheckTreeVisibility(rootTree, rootTree.X, neighborTree.Y + 1, Direction.Down);

    }
}

void SetDistanceToLastVisibleTree(Tree rootTree, Tree lastVisibleTree, string direction)
{
    switch(direction)
    {
        case Direction.Left:
            rootTree.ViewingDistanceLeft = rootTree.X - lastVisibleTree.X;
            break;
        case Direction.Right:
            rootTree.ViewingDistanceRight = lastVisibleTree.X - rootTree.X;
            break;
        case Direction.Up:
            rootTree.ViewingDistanceTop = rootTree.Y - lastVisibleTree.Y;
            break;
        case Direction.Down:
            rootTree.ViewingDistanceBottom = lastVisibleTree.Y - rootTree.Y;
            break;
    }

}


public record class Tree 
{
    public readonly int X;
    public readonly int Y;
    public readonly int Value;

    public int ViewingDistanceLeft = 0;
    public int ViewingDistanceRight = 0;
    public int ViewingDistanceTop = 0;
    public int ViewingDistanceBottom = 0;

    public int ScenicScore => ViewingDistanceLeft * ViewingDistanceRight * ViewingDistanceTop * ViewingDistanceBottom;
    
    public bool IsVisibleFromRightEdge = false;
    public bool IsVisibleFromLeftEdge = false;
    public bool IsVisibleFromTopEdge = false;
    public bool IsVisibleFromBottomEdge = false;

    public bool IsVisible => IsVisibleFromRightEdge || IsVisibleFromLeftEdge || IsVisibleFromTopEdge || IsVisibleFromBottomEdge;

    public Tree(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
}

record struct Direction() 
{
    public const string Up = "up";
    public const string Down = "down";
    public const string Left = "left";
    public const string Right = "right";
};