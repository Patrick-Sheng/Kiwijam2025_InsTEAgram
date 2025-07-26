using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMView : MonoBehaviour
{
    [SerializeField]
    private Screen screen;

    [SerializeField]
    private Sprite[] headerSprites = new Sprite[9];
    [SerializeField]
    private GameManager.CHARACTERS[] headerChars = new GameManager.CHARACTERS[9];
    public Dictionary<GameManager.CHARACTERS, Sprite> headers = new Dictionary<GameManager.CHARACTERS, Sprite>();

    [SerializeField]
    private Image headerImage;

    public void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            headers[headerChars[i]] = headerSprites[i];
        }
        screen.onChangeScreen.AddListener(ChangeCharacter);
    }
    public void ChangeCharacter()
    {
        headerImage.sprite = headers[screen.character];
    }
}
