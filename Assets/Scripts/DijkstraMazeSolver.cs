using System;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraMazeSolver : MazeSolverBase
{
    public override List<Vector2Int> SolveMaze(Walls[,] mazeArg, Vector2Int start, Vector2Int end)
    {
        maze = mazeArg;
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);
        int[,] distance = new int[width, height];
        bool[,] visited = new bool[width, height];
        Vector2Int[,] previous = new Vector2Int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                distance[x, y] = int.MaxValue;
                visited[x, y] = false;
            }
        }

        distance[start.x, start.y] = 0;
        while (true)
        {
            Vector2Int current = MinDistance(distance, visited);
            if (current == end || distance[current.x, current.y] == int.MaxValue)
                break;

            visited[current.x, current.y] = true;

            List<Vector2Int> neighbors = GetNeighbors(current);
            foreach (Vector2Int neighbor in neighbors)
            {
                int alt = distance[current.x, current.y] + 1;
                if (alt < distance[neighbor.x, neighbor.y])
                {
                    distance[neighbor.x, neighbor.y] = alt;
                    previous[neighbor.x, neighbor.y] = current;
                }
            }
        }

        if (distance[end.x, end.y] != int.MaxValue)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int currentNode = end;
            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = previous[currentNode.x, currentNode.y];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        return null; // No path found
    }

    private Vector2Int MinDistance(int[,] distance, bool[,] visited)
    {
        int min = int.MaxValue;
        Vector2Int minNode = new Vector2Int(-1, -1);

        for (int x = 0; x < distance.GetLength(0); x++)
        {
            for (int y = 0; y < distance.GetLength(1); y++)
            {
                if (!visited[x, y] && distance[x, y] < min)
                {
                    min = distance[x, y];
                    minNode = new Vector2Int(x, y);
                }
            }
        }

        return minNode;
    }
}
