using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataStorage : MonoBehaviour
{
    public static GameDataStorage Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public Vector2Int mazeSize;
    public int currentLevelNumber;
    public int totalNumberOfLevels;
}
