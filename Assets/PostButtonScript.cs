using UnityEngine;
using UnityEngine.UI;

public class PostButtonScript : MonoBehaviour
{
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
        GameManager.Instance.ChangeScreen(GameManager.SCREENS.POST, toCharacter);
    }
}
