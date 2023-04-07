using QuikGraph;
using QuikGraph.Algorithms;

namespace Day_12
{
    public class Service
    {
        public int GetShortestPath(string inputFile)
        {
            var heightMap = GetHeightMapFromInput(inputFile);

            // Initialize an empty dictionary that will represent the graph
            var dictionary = new Dictionary<int, int[]>();

            // Iterate over each cell in the height map
            var root = -1;
            var target = -1;
            for (var i = 0; i < heightMap.GetLength(0); i++)
            {
                for (var j = 0; j < heightMap.GetLength(1); j++)
                {
                    var c = heightMap[i, j];
                    var elevation = c - 'a';
                    var vertex = i * heightMap.GetLength(1) + j;

                    // Add the current cell to the dictionary as a vertex with the appropriate elevation level
                    dictionary[vertex] = Array.Empty<int>();

                    // Add the adjacent vertices to the dictionary with the appropriate elevation levels based on the given restrictions
                    var adjVertices = new List<int>();

                    if (c == 'S' || c == 'E')
                    {
                        elevation = c == 'S' ? 0 : 25;
                    }

                    if (i > 0 && Math.Abs((heightMap[i - 1, j] == 'S' ? 0 : (heightMap[i - 1, j] == 'E' ? 25 : heightMap[i - 1, j] - 'a')) - elevation) <= 1)
                    {
                        adjVertices.Add(vertex - heightMap.GetLength(1));
                    }

                    if (i < heightMap.GetLength(0) - 1 && Math.Abs((heightMap[i + 1, j] == 'S' ? 0 : (heightMap[i + 1, j] == 'E' ? 25 : heightMap[i + 1, j] - 'a')) - elevation) <= 1)
                    {
                        adjVertices.Add(vertex + heightMap.GetLength(1));
                    }

                    if (j > 0 && Math.Abs((heightMap[i, j - 1] == 'S' ? 0 : (heightMap[i, j - 1] == 'E' ? 25 : heightMap[i, j - 1] - 'a')) - elevation) <= 1)
                    {
                        adjVertices.Add(vertex - 1);
                    }

                    if (j < heightMap.GetLength(1) - 1 && Math.Abs((heightMap[i, j + 1] == 'S' ? 0 : (heightMap[i, j + 1] == 'E' ? 25 : heightMap[i, j + 1] - 'a')) - elevation) <= 1)
                    {
                        adjVertices.Add(vertex + 1);
                    }

                    dictionary[vertex] = adjVertices.ToArray();

                    if (c == 'S')
                    {
                        root = vertex;
                    }
                    else if (c == 'E')
                    {
                        target = vertex;
                    }
                }
            }

            var graph = dictionary.ToDelegateVertexAndEdgeListGraph(
                kv => Array.ConvertAll(kv.Value, v => new Edge<int>(kv.Key, v)));

            // Find the shortest path
            var tryGetPath = graph.ShortestPathsDijkstra(e => 1, root);

            if (tryGetPath(target, out var path))
            {
                var minSteps = path.Count();

                Console.WriteLine($"The shortest path has {minSteps}");

                return minSteps;
            }

            return 0;
        }

        private static char[,] GetHeightMapFromInput(string inputFile)
        {
            // Read the file contents into a string array
            var lines = File.ReadAllLines(inputFile);

            // Determine the dimensions of the 2D char array
            var numRows = lines.Length;
            var numCols = lines[0].Length;

            // Create the 2D char array
            var charArray = new char[numRows, numCols];

            // Populate the char array with the file contents
            for (var i = 0; i < numRows; i++)
            {
                var line = lines[i];
                for (var j = 0; j < numCols; j++)
                {
                    if (j >= line.Length)
                    {
                        // If the line is shorter than the expected number of columns,
                        // fill in the remaining cells with a space character
                        charArray[i, j] = ' ';
                    }
                    else
                    {
                        charArray[i, j] = line[j];
                    }
                }
            }


            return charArray;
        }

        //1. Read in the input one character at a time.
        //    For each input record the ID, Coordinates, AsciiValue. Dictionary<tuple<int, int>, record: ID, asciiValue>
        //Keep track of the root and target nodes S/E.
        //    Track Max X and Max Y being the 
        //2. Second pass to create vertex and adjacent node dictionary.evaluate coordinates and ascii value
        //3. Convert to graph with the library extension method.
        //4. Call method to solve the problem celebrate with hoots and howls.
    }
}



