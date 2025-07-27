using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameManager.SCREENS toScreen = GameManager.SCREENS.DMLIST;
    public GameManager.CHARACTERS toCharacter = GameManager.CHARACTERS.GEORGE;
    [SerializeField]
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (toCharacter == GameManager.CHARACTERS.REUNION || toCharacter == GameManager.CHARACTERS.NULL)
        {
            if (toScreen == GameManager.SCREENS.PROFILE || toScreen == GameManager.SCREENS.POST)
            {
                return;
            }

        }
        GameManager.Instance.ChangeScreen(toScreen, toCharacter);
        Debug.Log("Change to Character: " + toCharacter.ToString());
    }
}
