using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

enum RandomEnum
{
    Default,
    Perlin,
    Wevelet,
}

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private RandomEnum randomNumberProvider;

    [SerializeField] private float cellSize = 1f;

    [SerializeField] private Transform wallPrefab = null;

    [SerializeField] private Transform holePrefab = null;

    [SerializeField] private bool isFakeMaze = false;
    private Vector2Int fakeMazeSize = new Vector2Int(64, 64);

    private GameDataStorage levelData;
    private RandomNumberProviderBase rngProvider;
    private List<Vector2Int> path;

    private RandomNumberProviderBase GetRandomNumberProvider()
    {
        switch(randomNumberProvider)
        {
            case RandomEnum.Default:
                return new DefaultRandom();
            case RandomEnum.Perlin:
                return new PerlinNoiseRandom();
            case RandomEnum.Wevelet:
                return new WeveletNoiseRandom();
        }
        return new DefaultRandom();
    }

    private void CreateRandomNumberProvider()
    {
        switch (randomNumberProvider)
        {
            case RandomEnum.Default:
                rngProvider = new DefaultRandom();
                return;
        }
        rngProvider = new DefaultRandom();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateRandomNumberProvider();

        MazeGenerator.SetRNGProvider(GetRandomNumberProvider());

        levelData = GameDataStorage.Instance;

        CreateNewMaze();
    }

    public void CreateNewMaze()
    {
        DeleteOldMaze();

        var mazeSize = isFakeMaze ? fakeMazeSize : levelData.mazeSize;

        var startingPosition = new Vector2Int(rngProvider.GetRandomIntAtPos(0, mazeSize.x - 1, 0, 0), rngProvider.GetRandomIntAtPos(0, mazeSize.y - 1, 0, 0));
        var maze = MazeGenerator.GenerateMaze(mazeSize, startingPosition);

        if (!isFakeMaze)
        {
            if (player != null)
                player.position = LogicToWorld(startingPosition);

            var finishPosition = GetRandomFinishPosition(maze, startingPosition);
            if (holePrefab != null)
            {
                var hole = Instantiate(holePrefab, transform);
                hole.transform.position = LogicToWorld(finishPosition);
                hole.GetComponentInChildren<Hole>().SetMazeRenderer(this);
            }

            var solver = new AStarMazeSolver();
            path = solver.SolveMaze(maze, startingPosition, finishPosition);

            StartCoroutine(RepeatActionWithDelay());
        }

        Draw(maze);
    }
    private IEnumerator RepeatActionWithDelay()
    {
        while (path.Count > 0)
        {
            player.position = LogicToWorld(path.First());
            path.RemoveAt(0);

            if(path.Count > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
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
        var mazeSize = isFakeMaze ? fakeMazeSize : levelData.mazeSize;
        var position = new Vector3(-Mathf.FloorToInt(mazeSize.x * 0.5f) + pos.x, -Mathf.FloorToInt(mazeSize.y * 0.5f) + pos.y, 0) + new Vector3(0, cellSize * 0.5f, 0);
        return position;
    }
    private void Draw(Walls[,] maze)
    {
        var mazeSize = isFakeMaze ? fakeMazeSize : levelData.mazeSize;

        var halfCellSize = cellSize * 0.5f;
        var halfMazeSizeX = Mathf.FloorToInt(mazeSize.x * 0.5f);
        var halfMazeSizeY = Mathf.FloorToInt(mazeSize.y * 0.5f);
        for (int i = 0; i < mazeSize.x; ++i)
        {
            {
                var cell = maze[i, 0];
                var position = new Vector3(-halfMazeSizeX + i, -halfMazeSizeY, 0);
                if (cell.HasFlag(Walls.DOWN))
                {
                    var down = Instantiate(wallPrefab, transform);
                    down.transform.position = position + new Vector3(0, -halfCellSize, 0);
                    down.transform.localScale = new Vector3(cellSize, down.transform.localScale.y, down.transform.localScale.z);
                }
            }
            
            for (int j = 0; j < mazeSize.y; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-halfMazeSizeX + i, -halfMazeSizeY + j, 0);

                if(cell.HasFlag(Walls.UP))
                {
                    var top = Instantiate(wallPrefab, transform);
                    top.transform.position = position + new Vector3(0, halfCellSize, 0);
                    top.transform.localScale = new Vector3(cellSize, top.transform.localScale.y, top.transform.localScale.z);
                }
                if(cell.HasFlag(Walls.LEFT))
                {
                    var left = Instantiate(wallPrefab, transform);
                    left.transform.position = position + new Vector3(-halfCellSize, 0, 0);
                    left.transform.eulerAngles = new Vector3(0, 0, 90);
                    left.transform.localScale = new Vector3(cellSize, left.transform.localScale.y, left.transform.localScale.z);
                }

                if (i == mazeSize.x - 1)
                {
                    if (cell.HasFlag(Walls.RIGHT))
                    {
                        var right = Instantiate(wallPrefab, transform);
                        right.transform.position = position + new Vector3(halfCellSize, 0, 0);
                        right.transform.eulerAngles = new Vector3(0, 0, 90);
                        right.transform.localScale = new Vector3(cellSize, right.transform.localScale.y, right.transform.localScale.z);
                    }
                }
            }
        }
    }
}
