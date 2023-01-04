

string[] lines = System.IO.File.ReadAllLines(@"./day-08-input.txt");
var grid = new Dictionary<string, Tree>();
int MAX_X = lines[0].Length - 1;
int MAX_Y = lines.Length - 1;

var row = 0;
var column = 0;
foreach (var line in lines)
{
    column = 0;
    foreach(var number in line)
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

int recursiveTreeCheckCount = 0; // To compare calls with/without optimizations

findVisibleTrees(grid, startX: 0, startY: 0, endX: MAX_X, endY: MAX_Y);

var visibleTreeCount = grid.Select(x => x.Value).Where(t => t.IsVisible).Count();
var highestScenicScore = grid.Select(x => x.Value.ScenicScore).Max();

Console.WriteLine($"Visible trees from outside the grid: {visibleTreeCount}");
Console.WriteLine($"Highest scenic score: {highestScenicScore}");
// Console.WriteLine(recursiveTreeCheckCount); 

void findVisibleTrees(Dictionary<string, Tree> grid, int startX, int startY, int endX, int endY)
{
    var height = endY - startY;
    if (height == 0) // We are down to a grid of 1 x 5
    {
        var width = endX - startX;
        if (width == 0) // We are down to a grid of 1 x 1
        {
            var currentTree = grid.GetValueOrDefault($"{startX},{startY}");
            if (!currentTree.IsVisibleFromLeftEdge)
                currentTree.IsVisibleFromLeftEdge = CheckTreeVisibility(currentTree, currentTree.X - 1, currentTree.Y, "left");
            
            if (!currentTree.IsVisibleFromRightEdge)
                currentTree.IsVisibleFromRightEdge = CheckTreeVisibility(currentTree, currentTree.X + 1, currentTree.Y, "right");
            
            if (!currentTree.IsVisibleFromTopEdge)
                currentTree.IsVisibleFromTopEdge = CheckTreeVisibility(currentTree, currentTree.X, currentTree.Y - 1, "up");
            
            if (!currentTree.IsVisibleFromBottomEdge)
                currentTree.IsVisibleFromBottomEdge = CheckTreeVisibility(currentTree, currentTree.X, currentTree.Y + 1, "down");

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
    recursiveTreeCheckCount++;
    
    var neighborTree = grid.GetValueOrDefault($"{nextX},{nextY}");
    var rootLargerThanNextNeighbor = rootTree.Value > neighborTree.Value;
    var rootEqualToNextNeighbor = rootTree.Value == neighborTree.Value;

    if (!rootLargerThanNextNeighbor || rootEqualToNextNeighbor)
    {
        switch(direction)
        {
            case "left":
                rootTree.ViewingDistanceLeft = rootTree.X - neighborTree.X;
                break;
            case "right":
                rootTree.ViewingDistanceRight = neighborTree.X - rootTree.X;
                break;
            case "up":
                rootTree.ViewingDistanceTop = rootTree.Y - neighborTree.Y;
                break;
            case "down":
                rootTree.ViewingDistanceBottom = neighborTree.Y - rootTree.Y;
                break;
        }

        return false;
    }

    switch(direction)
    {
        case "left":
            if (neighborTree.X == 0 || neighborTree.IsVisibleFromLeftEdge) // Neighbor is at left edge
            { 
                rootTree.ViewingDistanceLeft = rootTree.X;
                return true;
            }

            return CheckTreeVisibility(rootTree, neighborTree.X - 1, rootTree.Y, "left");

        case "right":
            if (neighborTree.X == MAX_X || neighborTree.IsVisibleFromRightEdge) // Neighbor is at right edge
            {
                rootTree.ViewingDistanceRight = MAX_X - rootTree.X;
                return true;
            }

            return CheckTreeVisibility(rootTree, neighborTree.X + 1, rootTree.Y, "right");

        case "up":
            if (neighborTree.Y == 0 || neighborTree.IsVisibleFromTopEdge) // Neighbor is at top edge
            {
                rootTree.ViewingDistanceTop = rootTree.Y;
                return true;
            }

            return CheckTreeVisibility(rootTree, rootTree.X, neighborTree.Y - 1, "up");

        default:
            if (neighborTree.Y == MAX_Y || neighborTree.IsVisibleFromBottomEdge) // Neighbor is at top edge
            {
                rootTree.ViewingDistanceBottom = MAX_Y - rootTree.Y;
                return true;
            }
                    
            return CheckTreeVisibility(rootTree, rootTree.X, neighborTree.Y + 1, "down");

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