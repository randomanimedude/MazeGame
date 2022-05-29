using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] string coreGameSceneName = "CoreGame";

    public void ButtonStart()
    {
        SceneManager.LoadScene(coreGameSceneName);
    }
}
