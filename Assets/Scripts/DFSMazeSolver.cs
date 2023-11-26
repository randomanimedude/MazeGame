using System;
using System.Collections.Generic;
using UnityEngine;

// Breadth-First Search
public class DFSMazeSolver : BFSMazeSolver
{
    public override List<Vector2Int> SolveMaze(Walls[,] mazeArg, Vector2Int start, Vector2Int end)
    {
        maze = mazeArg;
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        bool[,] visited = new bool[width, height];
        Vector2Int[,] previous = new Vector2Int[width, height];

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(start);
        visited[start.x, start.y] = true;

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();

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
                    stack.Push(neighbor);
                }
            }
        }

        return null; // No path found
    }
}
