using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    private MazeRenderer mazeRenderer = null;
    [SerializeField] private string mainSceneName = "Menu";

    public void SetMazeRenderer(MazeRenderer renderer)
    {
        mazeRenderer = renderer;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        EndLevel();
    }

    private void EndLevel()
    {
        if (mazeRenderer == null)
            return;

        if(++GameDataStorage.Instance.currentLevelNumber > GameDataStorage.Instance.totalNumberOfLevels)
        {
            //switch to menu
            LoadMenu();
        }

        mazeRenderer.CreateNewMaze();
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
