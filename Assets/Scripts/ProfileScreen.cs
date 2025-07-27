using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScreen : MonoBehaviour
{
    [SerializeField]
    private Screen screen;

    [SerializeField]
    private PostsScreen postScreen;

    [SerializeField]
    private Image posts;

    [SerializeField]
    private GameObject tagToggleCover;

    [SerializeField]
    private Image bioHeader;

    [SerializeField]
    private Button tagToggleButton;

    [SerializeField]
    private Button postsButton;

    [SerializeField]
    private ButtonScript backButton;

    [SerializeField]
    private GameManager.CHARACTERS[] characters = new GameManager.CHARACTERS[9];

    [SerializeField]
    private Sprite[] bioHeaders = new Sprite[9];

    [SerializeField]
    private Sprite[] normPosts = new Sprite[9];

    [SerializeField]
    private Sprite[] tagPosts = new Sprite[9];

    private Dictionary<GameManager.CHARACTERS, Sprite> bioDict = new Dictionary<GameManager.CHARACTERS, Sprite>();
    private Dictionary<GameManager.CHARACTERS, Sprite> normPostDict = new Dictionary<GameManager.CHARACTERS, Sprite>();
    private Dictionary<GameManager.CHARACTERS, Sprite> tagPostsDict = new Dictionary<GameManager.CHARACTERS, Sprite>();

    private bool isTag = false;

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            bioDict[characters[i]] = bioHeaders[i];
            normPostDict[characters[i]] = normPosts[i];
            tagPostsDict[characters[i]] = tagPosts[i];
        }
        screen.onChangeScreen.AddListener(OnChanged);
        tagToggleButton.onClick.AddListener(OnTagToggled);
        postsButton.onClick.AddListener(OnPostButtonPressed);
        OnChanged();
    }

    private void OnChanged()
    {
        backButton.toCharacter = screen.character;
        bioHeader.sprite = bioDict[screen.character];
        if (isTag)
        {
            OnTagToggled();
        }
        posts.sprite = normPostDict[screen.character];
        if (GameManager.Instance.lastScreen != GameManager.SCREENS.DMLIST)
        {
            backButton.toScreen = GameManager.SCREENS.DMVIEW;
        }
        else
        {
            backButton.toScreen = GameManager.SCREENS.DMLIST;
        }
    }


    private void OnTagToggled()
    {
        isTag = !isTag;
        GameManager.Instance.isTag = isTag;
        tagToggleCover.SetActive(isTag);
        if (isTag)
        {
            posts.sprite = tagPostsDict[screen.character];
        }
        else
        {
            posts.sprite = normPostDict[screen.character];
        }
    }

    private void OnPostButtonPressed()
    {
        if (isTag)
        {
            if (screen.character == GameManager.CHARACTERS.JOEY || screen.character == GameManager.CHARACTERS.BRYAN)
            {
                GameManager.Instance.ChangeScreen(GameManager.SCREENS.POST, screen.character);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (screen.character == GameManager.CHARACTERS.JOEY)
            {
                return;
            }
            else
            {
                GameManager.Instance.ChangeScreen(GameManager.SCREENS.POST, screen.character);
            }
        }
    }
}
