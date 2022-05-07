using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Flags]
public enum Walls
{
    UP = 1,
    DOWN = 2,
    LEFT = 4,
    RIGHT = 8,

    VISITED = 16,
}

public static class MazeGenerator
{
    private static RandomNumberProviderBase randomNumberProvider;
    public static void SetRNGProvider(RandomNumberProviderBase rng) { randomNumberProvider = rng; }

    private static Vector2Int directionTop = new Vector2Int(0, 1);
    private static Vector2Int directionDown = new Vector2Int(0, -1);
    private static Vector2Int directionLeft = new Vector2Int(-1, 0);
    private static Vector2Int directionRight = new Vector2Int(1, 0);

    private static List<Vector2Int> directions = new List<Vector2Int> { directionTop, directionDown, directionLeft, directionRight };


    public static Walls[,] GenerateMaze(Vector2Int mazeSize, Vector2Int startingPosition)
    {
        randomNumberProvider.InitRandom();


        Walls[,] walls = new Walls[mazeSize.x, mazeSize.y];

        var fourWalls = Walls.UP | Walls.DOWN | Walls.LEFT | Walls.RIGHT;

        for (int i = 0; i < mazeSize.x; ++i)
        {
            for (int j = 0; j < mazeSize.y; ++j)
            {
                walls[i, j] = fourWalls;
            }
        }

        List<Vector2Int> stack = new List<Vector2Int>();
        stack.Add(startingPosition);
        walls[startingPosition.x, startingPosition.y] |= Walls.VISITED;

        while (stack.Count > 0)
        {
            var currentCell = stack.Last();
            List<Vector2Int> possibleNeighbours = new List<Vector2Int>();


            directions.ForEach(delegate (Vector2Int direction)
            {
                Vector2Int neighbour = currentCell + direction;
                if (neighbour.x >= 0 && neighbour.x < mazeSize.x
                && neighbour.y >= 0 && neighbour.y < mazeSize.y)
                {
                    if(!walls[neighbour.x, neighbour.y].HasFlag(Walls.VISITED))
                    {
                        if (direction == directionTop)
                        {
                            possibleNeighbours.Add(direction);
                            return;
                        }
                        if (direction == directionDown)
                        {
                            possibleNeighbours.Add(direction);
                            return;
                        }
                        if (direction == directionLeft)
                        {
                            possibleNeighbours.Add(direction);
                            return;
                        }
                        if (direction == directionRight)
                        {
                            possibleNeighbours.Add(direction);
                            return;
                        }
                    }
                    
                }
            });

            if (possibleNeighbours.Count > 0)
            {
                var direction = possibleNeighbours[randomNumberProvider.GetRandomInt(0, possibleNeighbours.Count - 1)];
                var neighbour = currentCell + direction;
                if (direction == directionTop)
                {
                    walls[currentCell.x,currentCell.y] ^= Walls.UP;
                    walls[neighbour.x, neighbour.y] ^= Walls.DOWN;
                }
                if (direction == directionDown)
                {
                    walls[currentCell.x, currentCell.y] ^= Walls.DOWN;
                    walls[neighbour.x, neighbour.y] ^= Walls.UP;
                }
                if (direction == directionLeft)
                {
                    walls[currentCell.x, currentCell.y] ^= Walls.LEFT;
                    walls[neighbour.x, neighbour.y] ^= Walls.RIGHT;
                }
                if (direction == directionRight)
                {
                    walls[currentCell.x, currentCell.y] ^= Walls.RIGHT;
                    walls[neighbour.x, neighbour.y] ^= Walls.LEFT;
                }

                walls[neighbour.x, neighbour.y] |= Walls.VISITED;

                stack.Add(neighbour);
                continue;
            }

            stack.Remove(stack.Last());
        }

        return walls;
    }
}
