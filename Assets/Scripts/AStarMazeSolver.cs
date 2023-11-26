using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A* Search
public class AStarMazeSolver : MazeSolverBase
{
    private class PriorityQueue<T>
    {
        private SortedDictionary<float, Queue<T>> elements = new SortedDictionary<float, Queue<T>>();

        public int Count { get; private set; }

        public bool Contains(T item)
        {
            foreach (var kvp in elements)
            {
                if (kvp.Value.Contains(item))
                    return true;
            }
            return false;
        }

        public void Enqueue(T item, float priority)
        {
            if (!elements.ContainsKey(priority))
            {
                elements[priority] = new Queue<T>();
            }

            elements[priority].Enqueue(item);
            Count++;
        }

        public T Dequeue()
        {
            var item = elements.First();
            var queue = item.Value;
            var removed = queue.Dequeue();
            if (queue.Count == 0)
            {
                elements.Remove(item.Key);
            }
            Count--;
            return removed;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override List<Vector2Int> SolveMaze(Walls[,] mazeArg, Vector2Int start, Vector2Int end)
    {
        maze = mazeArg;
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        List<Vector2Int> path = new List<Vector2Int>();
        Dictionary<Vector2Int, float> gScores = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> fScores = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        // Initialize gScores and fScores with default values
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gScores[new Vector2Int(x, y)] = float.MaxValue;
                fScores[new Vector2Int(x, y)] = float.MaxValue;
            }
        }

        gScores[start] = 0;
        fScores[start] = HeuristicCostEstimate(start, end);

        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        openSet.Enqueue(start, fScores[start]);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == end)
            {
                return ReconstructPath(cameFrom, start, end);
            }

            List<Vector2Int> neighbors = GetNeighbors(current);

            foreach (Vector2Int neighbor in neighbors)
            {
                float tentativeGScore = gScores[current] + 1; // Assuming uniform edge weight of 1

                if (tentativeGScore < gScores[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScores[neighbor] = tentativeGScore;
                    fScores[neighbor] = gScores[neighbor] + HeuristicCostEstimate(neighbor, end);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScores[neighbor]);
                    }
                }
            }
        }

        return null; // No path found
    }
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    // Helper function to calculate the heuristic cost estimate (Euclidean distance)
    private float HeuristicCostEstimate(Vector2Int start, Vector2Int end)
    {
        return Vector2Int.Distance(start, end);
    }
}
