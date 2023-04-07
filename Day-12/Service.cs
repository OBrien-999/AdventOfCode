using QuikGraph;
using QuikGraph.Algorithms;

namespace Day_12
{
    public class Service
    {
        public int GetShortestPath()
        {
            // Parse the height map into a 2D array of characters
            var heightMap = new[,]
            {
                {'S', 'a', 'b', 'q', 'p', 'o', 'n', 'm'},
                {'a', 'b', 'c', 'r', 'y', 'x', 'x', 'l'},
                {'a', 'c', 'c', 's', 'z', 'E', 'x', 'k'},
                {'a', 'c', 'c', 't', 'u', 'v', 'w', 'j'},
                {'a', 'b', 'd', 'e', 'f', 'g', 'h', 'i'}
            };

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

            if (tryGetPath(target, out IEnumerable<Edge<int>> path))
            {
                var minSteps = path.Count();

                Console.WriteLine($"The shortest path has {minSteps}");

                return minSteps;
            }

            return 0;
        }
    }
}



