using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public enum SCREENS
    {
        DMLIST,
        DMVIEW,
        PROFILE,
        POST,
    }

    public Dictionary<SCREENS, GameObject> screenDictionary = new Dictionary<SCREENS, GameObject>();
    public List<GameObject> screenObjectArray = new List<GameObject>();
    
    public enum CHARACTERS
    {
        GEORGE,
        BRYAN,
        CLARA,
        EDDIE,
        FIONA,
        HANNAH,
        ISABEL,
        JOEY,
        DAVID,
        REUNION,
        NULL,
    }

    public SCREENS lastScreen = SCREENS.DMLIST;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void AddToScreens(GameObject go)
    {
        screenObjectArray.Add(go);
        if (screenObjectArray.Count >= 4)
        {
            ChangeScreen(SCREENS.DMLIST, CHARACTERS.GEORGE);
        }
    }

    public void ChangeScreen(SCREENS toScreen, CHARACTERS toCharacter)
    {
        for (int i = 0; i < screenObjectArray.Count; i++)
        {
            if (screenObjectArray[i].activeSelf)
            {
                lastScreen = screenObjectArray[i].GetComponent<Screen>().ScreenType;
            }
            screenObjectArray[i].SetActive(false);
        }
        screenDictionary[toScreen].SetActive(true);
        screenDictionary[toScreen].GetComponent<Screen>().Activate(toCharacter);
    }
}
