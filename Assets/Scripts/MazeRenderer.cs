using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] private RandomNumberProviderBase randomNumberProvider;

    [SerializeField] private Vector2Int mazeSize = new Vector2Int(10, 10);
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private Transform wallPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator.SetRNGProvider(randomNumberProvider);
        var maze = MazeGenerator.GenerateMaze(mazeSize, new Vector2Int(0, 0));
        Draw(maze);
    }

    private void Draw(Walls[,] maze)
    {
        for (int i = 0; i < mazeSize.x; ++i)
        {
            for (int j = 0; j < mazeSize.y; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-mazeSize.x / 2 + i, 0, -mazeSize.y / 2 + j);

                if(cell.HasFlag(Walls.UP))
                {
                    var top = Instantiate(wallPrefab, transform);
                    top.transform.position = position + new Vector3(0, 0, cellSize / 2);
                    top.transform.localScale = new Vector3(cellSize, top.transform.localScale.y, top.transform.localScale.z);
                }
                if(cell.HasFlag(Walls.LEFT))
                {
                    var left = Instantiate(wallPrefab, transform);
                    left.transform.position = position + new Vector3(-cellSize / 2, 0, 0);
                    left.transform.eulerAngles = new Vector3(0, 90, 0);
                    left.transform.localScale = new Vector3(cellSize, left.transform.localScale.y, left.transform.localScale.z);
                }

                if (j ==/* mazeSize.y - 1*/0)
                {
                    if (cell.HasFlag(Walls.DOWN))
                    {
                        var down = Instantiate(wallPrefab, transform);
                        down.transform.position = position + new Vector3(0, 0, -cellSize / 2);
                        down.transform.localScale = new Vector3(cellSize, down.transform.localScale.y, down.transform.localScale.z);
                    }
                }
                if (i == mazeSize.x - 1)
                {
                    if (cell.HasFlag(Walls.RIGHT))
                    {
                        var right = Instantiate(wallPrefab, transform);
                        right.transform.position = position + new Vector3(+cellSize / 2, 0, 0);
                        right.transform.eulerAngles = new Vector3(0, 90, 0);
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
