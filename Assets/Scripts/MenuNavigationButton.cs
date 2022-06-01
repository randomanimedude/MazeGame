using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigationButton : MonoBehaviour
{
    public Button button;
    public List<Transform> nodesToHide;
    public List<Transform> nodesToShow;

    void OnEnable()
    {
        //Register Button Events
        button.onClick.AddListener(() => buttonCallBack());
    }

    private void buttonCallBack()
    {
        foreach(Transform t in nodesToShow)
        {
            t.gameObject.SetActive(true);
        }

        foreach(Transform t in nodesToHide)
        {
            t.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        //Un-Register Button Events
        button.onClick.RemoveAllListeners();
    }
}
