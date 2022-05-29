using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Random
{
    Default,

}


public class MazeRenderer : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Random randomNumberProvider;

    [SerializeField] private Vector2Int mazeSize = new Vector2Int(10, 10);
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private Transform wallPrefab = null;

    [SerializeField] private Transform holePrefab = null;

    private RandomNumberProviderBase GetRandomNumberProvider()
    {
        switch(randomNumberProvider)
        {
            case Random.Default:
                return new DefaultRandom();
        }
        return new DefaultRandom();
    }

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator.SetRNGProvider(GetRandomNumberProvider());

        CreateNewMaze();
    }

    public void CreateNewMaze()
    {
        DeleteOldMaze();

        var startingPosition = new Vector2Int(0, 0);
        var maze = MazeGenerator.GenerateMaze(mazeSize, startingPosition);
        if (player != null)
            player.position = LogicToWorld(startingPosition);

        if (holePrefab != null)
        {
            var finishPosition = GetRandomFinishPosition(maze, startingPosition);
            var hole = Instantiate(holePrefab, transform);
            hole.transform.position = LogicToWorld(finishPosition);
            //hole.transform.position = LogicToWorld(new Vector2Int(1, 0));
            hole.GetComponentInChildren<Hole>().SetMazeRenderer(this);
        }

        Draw(maze);
    }

    private void DeleteOldMaze()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Vector2Int GetRandomFinishPosition(Walls[,] maze, Vector2Int start)
    {
        List<Vector2Int> possiblePositions = new List<Vector2Int>();
        for (int i = 0; i < maze.GetLength(0); ++i)
        {
            if (Mathf.Abs(i - start.x) < maze.GetLength(0) / 3)
                continue;

            for (int j = 0; j < maze.GetLength(1); ++j)
            {
                if (Mathf.Abs(j - start.y) < maze.GetLength(1) / 3)
                    continue;

                int numberOfWalls = 0;
                if (maze[i, j].HasFlag(Walls.UP))
                    ++numberOfWalls;
                if (maze[i, j].HasFlag(Walls.DOWN))
                    ++numberOfWalls;
                if (maze[i, j].HasFlag(Walls.LEFT))
                    ++numberOfWalls;
                if (maze[i, j].HasFlag(Walls.RIGHT))
                    ++numberOfWalls;

                if (numberOfWalls == 3)
                    possiblePositions.Add(new Vector2Int(i, j));
            }
        }

        return possiblePositions[UnityEngine.Random.Range(0, possiblePositions.Count)];
    }

    private Vector2 LogicToWorld(Vector2Int pos)
    {
        var position = new Vector3(-mazeSize.x / 2 + pos.x, -mazeSize.y / 2 + pos.y, 0) + new Vector3(0, cellSize / 2, 0);
        return position;
    }

    private void Draw(Walls[,] maze)
    {
        for (int i = 0; i < mazeSize.x; ++i)
        {
            for (int j = 0; j < mazeSize.y; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-mazeSize.x / 2 + i, -mazeSize.y / 2 + j, 0);

                if(cell.HasFlag(Walls.UP))
                {
                    var top = Instantiate(wallPrefab, transform);
                    top.transform.position = position + new Vector3(0, cellSize / 2, 0);
                    top.transform.localScale = new Vector3(cellSize, top.transform.localScale.y, top.transform.localScale.z);
                }
                if(cell.HasFlag(Walls.LEFT))
                {
                    var left = Instantiate(wallPrefab, transform);
                    left.transform.position = position + new Vector3(-cellSize / 2, 0, 0);
                    left.transform.eulerAngles = new Vector3(0, 0, 90);
                    left.transform.localScale = new Vector3(cellSize, left.transform.localScale.y, left.transform.localScale.z);
                }

                if (j ==/* mazeSize.y - 1*/0)
                {
                    if (cell.HasFlag(Walls.DOWN))
                    {
                        var down = Instantiate(wallPrefab, transform);
                        down.transform.position = position + new Vector3(0, -cellSize / 2, 0);
                        down.transform.localScale = new Vector3(cellSize, down.transform.localScale.y, down.transform.localScale.z);
                    }
                }
                if (i == mazeSize.x - 1)
                {
                    if (cell.HasFlag(Walls.RIGHT))
                    {
                        var right = Instantiate(wallPrefab, transform);
                        right.transform.position = position + new Vector3(+cellSize / 2, 0, 0);
                        right.transform.eulerAngles = new Vector3(0, 0, 90);
                        right.transform.localScale = new Vector3(cellSize, right.transform.localScale.y, right.transform.localScale.z);
                    }
                }
               
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
