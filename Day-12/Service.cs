using QuikGraph;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.ShortestPath;
using System.Collections.Generic;

namespace Day_12
{
    public class Service
    {
        public int GetShortestPath()
        {



            // Input heightmap
            string[] input = {
            "Sabqponm",
            "abcryxxl",
            "accszExk",
            "acctuvwj",
            "abdefghi"
        };

            int rows = input.Length;
            int cols = input[0].Length;

            (int, int) start = (-1, -1);
            (int, int) end = (-1, -1);

            // Find start and end positions
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char currentChar = input[i][j];
                    if (currentChar == 'S') start = (i, j);
                    if (currentChar == 'E') end = (i, j);
                }
            }

            // Dijkstra's algorithm to find the shortest path
            var distances = new Dictionary<(int, int), double>();
            var visited = new HashSet<(int, int)>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    distances[(i, j)] = double.PositiveInfinity;
                }
            }

            distances[start] = 0;

            while (visited.Count < rows * cols)
            {
                (int, int) minVertex = FindMinVertex(distances, visited);
                if (minVertex == end) break;

                visited.Add(minVertex);

                var adjacentCells = new List<(int, int)> { (minVertex.Item1 - 1, minVertex.Item2), (minVertex.Item1 + 1, minVertex.Item2), (minVertex.Item1, minVertex.Item2 - 1), (minVertex.Item1, minVertex.Item2 + 1) };
                foreach (var cell in adjacentCells)
                {
                    if (IsValidCell(cell, rows, cols) && !visited.Contains(cell))
                    {
                        char currentChar = input[minVertex.Item1][minVertex.Item2];
                        char adjacentChar = input[cell.Item1][cell.Item2];
                        if (Math.Abs(currentChar - adjacentChar) <= 1 || adjacentChar == 'E')
                        {
                            double newDistance = distances[minVertex] + 1;
                            if (newDistance < distances[cell])
                            {
                                distances[cell] = newDistance;
                            }
                        }
                    }
                }
            }

            // Calculate the fewest steps required
            if (distances[end] != double.PositiveInfinity)
            {
                Console.WriteLine($"The fewest steps required: {distances[end]}");
                return (int)distances[end];
            }
            else
            {
                Console.WriteLine("No path found.");
            }
            return -1;
            
        }

        // Finds the vertex with the minimum distance that hasn't been visited
        static (int, int) FindMinVertex(Dictionary<(int, int), double> distances, HashSet<(int, int)> visited)
        {
            double minValue = double.PositiveInfinity;
            (int, int) minVertex = (-1, -1);

            foreach (var item in distances)
            {
                if (!visited.Contains(item.Key) && item.Value < minValue)
                {
                    minValue = item.Value;
                    minVertex = item.Key;
                }
            }

            return minVertex;
        }

        // Checks if a cell is valid (inside the grid)
        static bool IsValidCell((int, int) cell, int rows, int cols)
                {
            return cell.Item1 >= 0 && cell.Item1 < rows && cell.Item2 >= 0 && cell.Item2 < cols;
        }
    }
    
}



  