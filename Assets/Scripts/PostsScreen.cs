using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostsScreen : MonoBehaviour
{
    [SerializeField]
    private ButtonScript backButton;

    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private Screen screen;

    [SerializeField]
    private Image headerImage;

    [SerializeField]
    private GameManager.CHARACTERS[] characters = new GameManager.CHARACTERS[9];

    [SerializeField]
    private float botBound = 0f;

    public bool[] hasNormPosts = new bool[9];

    public bool[] hasTagPosts = new bool[9];

    [SerializeField]
    private Sprite[] normHeaders = new Sprite[9];

    [SerializeField]
    private Sprite[] tagHeaders = new Sprite[9];

    [SerializeField]
    private GameObject[] normPosts = new GameObject[9];

    [SerializeField]
    private GameObject[] tagPosts = new GameObject[9];

    private Dictionary<GameManager.CHARACTERS, Sprite> useNormHeaders = new Dictionary<GameManager.CHARACTERS, Sprite>();
    private Dictionary<GameManager.CHARACTERS, Sprite> useTagHeader = new Dictionary<GameManager.CHARACTERS, Sprite>();
    private Dictionary<GameManager.CHARACTERS, GameObject> useNormPosts = new Dictionary<GameManager.CHARACTERS, GameObject>();
    private Dictionary<GameManager.CHARACTERS, GameObject> useTagPosts = new Dictionary<GameManager.CHARACTERS, GameObject>();

    private bool isTag = false;

    private GameObject activePost;
    private Image activePostImage;
    private float topMax;
    private float postHeightDiff;
    private Rect activePostRect;

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            if (hasNormPosts[i])
            {
                useNormHeaders[characters[i]] = normHeaders[i];
                useNormPosts[characters[i]] = normPosts[i];
                normPosts[i].SetActive(false);
            }
            if (hasTagPosts[i])
            {
                useTagHeader[characters[i]] = tagHeaders[i];
                useTagPosts[characters[i]] = tagPosts[i];
                tagPosts[i].SetActive(false);
            }

        }
        screen.onChangeScreen.AddListener(OnChanged);
        backButton.GetComponent<Button>().onClick.AddListener(OnExitPage);
    }

    private void Update()
    {
        activePostImage.rectTransform.anchoredPosition = new Vector3(0,topMax + (postHeightDiff*scrollbar.value), 0);
    }

    private void OnChanged()
    {
        backButton.toCharacter = screen.character;
        if (activePost)
        {
            activePost.SetActive(false);
        }

        if (GameManager.Instance.isTag)
        {
            headerImage.sprite = useTagHeader[screen.character];
            activePost = useTagPosts[screen.character];
            activePost.SetActive(true);
        }
        else
        {
            headerImage.sprite = useNormHeaders[screen.character];
            activePost = useNormPosts[screen.character];
            activePost.SetActive(true);
        }

        activePostImage = activePost.GetComponent<Image>();
        activePostRect = activePostImage.GetPixelAdjustedRect();
        topMax = activePostImage.rectTransform.anchoredPosition.y;
        scrollbar.size = 1 - ((activePostRect.height - 1095) / 3101);
        scrollbar.value = 0;
        postHeightDiff = activePostRect.height - botBound;
    }

    private void OnExitPage()
    {
        activePostImage.rectTransform.anchoredPosition = new Vector3(0, topMax, 0);
    }

}
