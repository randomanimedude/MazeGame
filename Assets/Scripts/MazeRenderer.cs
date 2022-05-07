using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Random
{
    Default,

}


public class MazeRenderer : MonoBehaviour
{
    [SerializeField] private Random randomNumberProvider;

    [SerializeField] private Vector2Int mazeSize = new Vector2Int(10, 10);
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private Transform wallPrefab = null;

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
        var maze = MazeGenerator.GenerateMaze(mazeSize, mazeSize / 2);
        Draw(maze);
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
