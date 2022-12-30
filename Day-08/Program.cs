

string[] lines = System.IO.File.ReadAllLines(@"./day-08-input.txt");
var grid = new Dictionary<string, Tree>();

var row = 0;
var column = 0;
foreach (var line in lines)
{
    column = 0;
    foreach(var number in line)
    {
        var tree = new Tree(column, row, Int32.Parse(number.ToString()));
        grid.Add(tree.ToString(), tree);
        column++;
    }

    row++;
}

var maxY = row - 1;
var maxX = column - 1;


findVisibleTrees(grid, startX: 0, startY: 0, endX: maxX, endY: maxY);

var visibleTreeCount = grid.Select(x => x.Value).Where(t => t.IsVisible).Count();

Console.WriteLine(visibleTreeCount);

void findVisibleTrees(Dictionary<string, Tree> grid, int startX, int startY, int endX, int endY)
{
    var height = endY - startY;
    if (height == 0) // We are down to a grid of 1 x 5
    {
        var width = endX - startX;
        if (width == 0) // We are down to a grid of 1 x 1
        {
            var currentTree = grid.GetValueOrDefault($"{startX},{startY}");
            if (currentTree.X == 0) // Tree is on left edge
            {
                currentTree.IsVisibleFromLeftEdge = true;
            } else {
                currentTree.IsVisibleFromLeftEdge = CheckTreeVisibilityLeft(currentTree, currentTree.X - 1, currentTree.Y);
            }
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

bool CheckTreeVisibilityLeft(Tree rootTree, int nextX, int nextY)
{
    var neighborTreeLeft = grid.GetValueOrDefault($"{nextX},{nextY}");
    var rootLargerThanNextNeighbor = rootTree.Value > neighborTreeLeft.Value;
    if (!rootLargerThanNextNeighbor)
        return false;
    
    if (neighborTreeLeft.X == 0)
    {
        return rootLargerThanNextNeighbor;
    }

    return CheckTreeVisibilityLeft(rootTree, neighborTreeLeft.X - 1, neighborTreeLeft.Y);
}

public record class Tree 
{
    public readonly int X;
    public readonly int Y;
    public readonly int Value;
    
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