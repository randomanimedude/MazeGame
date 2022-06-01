using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] string coreGameSceneName = "CoreGame";
    private GameDataStorage levelData;

    void Start()
    {
        levelData = GameDataStorage.Instance;
    }

    public void StartEasy()
    {
        levelData.mazeSize = new Vector2Int(7, 7);
        levelData.totalNumberOfLevels = 5;
        levelData.currentLevelNumber = 1;
        LoadLevel();
    }

    public void StartMedium()
    {
        levelData.mazeSize = new Vector2Int(10, 10);
        levelData.totalNumberOfLevels = 7;
        levelData.currentLevelNumber = 1;
        LoadLevel();
    }

    public void StartHard()
    {
        levelData.mazeSize = new Vector2Int(15, 15);
        levelData.totalNumberOfLevels = 10;
        levelData.currentLevelNumber = 1;
        LoadLevel();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(coreGameSceneName);
    }
}
