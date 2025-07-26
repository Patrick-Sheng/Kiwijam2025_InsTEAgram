using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField]
    private bool defaultScreen = false;
    [SerializeField]
    private GameManager.SCREENS ScreenType = GameManager.SCREENS.DMLIST;
    [SerializeField]
    private bool characterOwnership = false;
    [SerializeField]
    private GameManager.CHARACTERS character = GameManager.CHARACTERS.NULL;

    private void Start()
    {
        GameManager.Instance.screenDictionary.Add(ScreenType, this.gameObject);
        GameManager.Instance.AddToScreens(this.gameObject);
        Debug.Log(ScreenType.ToString());
    }

    public void Activate(GameManager.CHARACTERS _character)
    {
        character = _character;
    }
}
