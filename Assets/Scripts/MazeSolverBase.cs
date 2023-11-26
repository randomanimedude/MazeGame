using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeSolverBase
{
    protected Walls[,] maze;

    abstract public List<Vector2Int> SolveMaze(Walls[,] mazeArg, Vector2Int start, Vector2Int end);

    protected List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Assuming only UP, DOWN, LEFT, RIGHT walls are considered
        if (IsCellValid(current.x + 1, current.y) && !IsWall(current.x, current.y, Walls.RIGHT))
            neighbors.Add(new Vector2Int(current.x + 1, current.y));
        if (IsCellValid(current.x - 1, current.y) && !IsWall(current.x, current.y, Walls.LEFT))
            neighbors.Add(new Vector2Int(current.x - 1, current.y));
        if (IsCellValid(current.x, current.y + 1) && !IsWall(current.x, current.y, Walls.UP))
            neighbors.Add(new Vector2Int(current.x, current.y + 1));
        if (IsCellValid(current.x, current.y - 1) && !IsWall(current.x, current.y, Walls.DOWN))
            neighbors.Add(new Vector2Int(current.x, current.y - 1));

        return neighbors;
    }

    protected bool IsCellValid(int x, int y)
    {
        return x >= 0 && x < maze.GetLength(0) && y >= 0 && y < maze.GetLength(1);
    }

    protected bool IsWall(int x, int y, Walls wall)
    {
        return (maze[x, y] & wall) == wall;
    }
}
