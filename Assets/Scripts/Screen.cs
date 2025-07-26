using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Screen : MonoBehaviour
{
    [SerializeField]
    private bool defaultScreen = false;
    [SerializeField]
    private GameManager.SCREENS ScreenType = GameManager.SCREENS.DMLIST;
    [SerializeField]
    private bool characterOwnership = false;

    public GameManager.CHARACTERS character = GameManager.CHARACTERS.GEORGE;

    public UnityEvent onChangeScreen;

    private void Start()
    {
        if (onChangeScreen == null)
        {
            onChangeScreen = new UnityEvent();
        }

        GameManager.Instance.screenDictionary.Add(ScreenType, this.gameObject);
        GameManager.Instance.AddToScreens(this.gameObject);
        Debug.Log(ScreenType.ToString());
    }

    public void Activate(GameManager.CHARACTERS _character)
    {
        character = _character;
        onChangeScreen.Invoke();
    }
}
