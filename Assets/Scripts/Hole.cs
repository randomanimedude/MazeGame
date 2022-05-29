using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

    private MazeRenderer mazeRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMazeRenderer(MazeRenderer renderer)
    {
        mazeRenderer = renderer;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mazeRenderer == null)
            return;

        mazeRenderer.CreateNewMaze();
    }
}
