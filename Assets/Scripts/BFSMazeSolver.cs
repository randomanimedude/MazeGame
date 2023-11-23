using System;
using System.Collections.Generic;
using UnityEngine;

// Breadth-First Search
public class BFSMazeSolver : MazeSolverBase
{
    private Walls[,] maze;

    public override List<Vector2Int> SolveMaze(Walls[,] mazeArg, Vector2Int start, Vector2Int end)
    {
        maze = mazeArg;
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        bool[,] visited = new bool[width, height];
        Vector2Int[,] previous = new Vector2Int[width, height];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == end)
            {
                return ReconstructPath(previous, start, end);
            }

            List<Vector2Int> neighbors = GetNeighbors(current);

            foreach (Vector2Int neighbor in neighbors)
            {
                if (!visited[neighbor.x, neighbor.y])
                {
                    visited[neighbor.x, neighbor.y] = true;
                    previous[neighbor.x, neighbor.y] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return null; // No path found
    }
    private List<Vector2Int> ReconstructPath(Vector2Int[,] previous, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(current);
            current = previous[current.x, current.y];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}
