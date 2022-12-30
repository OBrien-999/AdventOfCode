

string[] lines = System.IO.File.ReadAllLines(@"./day-08-input.txt");
var grid = new Dictionary<string, Tree>();

var maxY = lines.Length - 1;
var maxX = lines[0].Length - 1;

var row = 0;
var column = 0;
foreach (var line in lines)
{
    column = 0;
    foreach(var number in line)
    {
        var tree = new Tree(column, row, Int32.Parse(number.ToString())) {
            IsVisibleFromLeftEdge = column == 0,
            IsVisibleFromRightEdge = column == maxX,
            IsVisibleFromTopEdge = row == 0,
            IsVisibleFromBottomEdge = row == maxY
        };
        grid.Add(tree.ToString(), tree);
        column++;
    }

    row++;
}

int recursiveTreeCheckCount = 0; // To compare calls with/without optimizations

findVisibleTrees(grid, startX: 0, startY: 0, endX: maxX, endY: maxY);

var visibleTreeCount = grid.Select(x => x.Value).Where(t => t.IsVisible).Count();

Console.WriteLine(visibleTreeCount);
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
    if (!rootLargerThanNextNeighbor)
        return false;
    
    
    if (direction == "left") {
        if (neighborTree.X == 0) // Neighbor is at left edge
            return rootLargerThanNextNeighbor;

        if (neighborTree.IsVisibleFromLeftEdge)
            return true;

        return CheckTreeVisibility(rootTree, neighborTree.X - 1, rootTree.Y, "left");
        
    } else if (direction == "right") {
        if (neighborTree.X == maxX) // Neighbor is at right edge
            return rootLargerThanNextNeighbor;

        if (neighborTree.IsVisibleFromRightEdge)
            return true;

        return CheckTreeVisibility(rootTree, neighborTree.X + 1, rootTree.Y, "right");
        
    } else if (direction == "up") {
        if (neighborTree.Y == 0) // Neighbor is at top edge
            return rootLargerThanNextNeighbor;

        if (neighborTree.IsVisibleFromTopEdge)
            return true;

        return CheckTreeVisibility(rootTree, rootTree.X, neighborTree.Y - 1, "up");

    } else {
        if (neighborTree.Y == maxY) // Neighbor is at top edge
            return rootLargerThanNextNeighbor;

        if (neighborTree.IsVisibleFromBottomEdge)
            return true;
                
        return CheckTreeVisibility(rootTree, rootTree.X, neighborTree.Y + 1, "down");
    }

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