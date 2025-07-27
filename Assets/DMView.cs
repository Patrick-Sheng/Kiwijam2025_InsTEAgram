using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DMView : MonoBehaviour
{
    [SerializeField]
    private Screen screen;

    [SerializeField]
    private Sprite[] headerSprites = new Sprite[10];

    [SerializeField]
    private GameManager.CHARACTERS[] headerChars = new GameManager.CHARACTERS[10];

    [SerializeField]
    private ButtonScript profileButton;

    [SerializeField]
    private GameObject[] dmGameObjects = new GameObject[10];

    public Dictionary<GameManager.CHARACTERS, GameObject> dmDictionary = new Dictionary<GameManager.CHARACTERS, GameObject>();

    public Dictionary<GameManager.CHARACTERS, Sprite> headers = new Dictionary<GameManager.CHARACTERS, Sprite>();

    [SerializeField]
    private Image headerImage;

    [SerializeField]
    private Scrollbar scrollbar;

    private GameObject activeChat;

    private bool canScroll = true;
    private Image activeChatImage;
    private float topMax = 0f;
    private float chatHeightDiff = 0f;
    private Rect chatImageRect;

    private GameManager.CHARACTERS lastCharacter;

    [SerializeField]
    private Button[] exitButtons = new Button[2];


    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            headers[headerChars[i]] = headerSprites[i];
            dmDictionary[headerChars[i]] = dmGameObjects[i];
        }
        screen.onChangeScreen.AddListener(ChangeCharacter);
        for (int i = 0; i < 2; i++)
        {
            exitButtons[i].onClick.AddListener(OnExitPage);
        }
    }

    private void Update()
    {
        if (!canScroll) { return; }

        activeChatImage.rectTransform.anchoredPosition = new Vector3(0,topMax + (chatHeightDiff*scrollbar.value), 0);
    }

    public void ChangeCharacter()
    {
        if (screen.character == GameManager.CHARACTERS.NULL)
        {
            screen.character = lastCharacter;
        }

        headerImage.sprite = headers[screen.character];
        for (int i = 0; i < dmGameObjects.Length; i++)
        {
            dmGameObjects[i].SetActive(false);
        }
        activeChat = dmDictionary[screen.character];
        activeChat.SetActive(true);
        activeChatImage = activeChat.GetComponent<Image>();
        //Use this var to get the size of the chat image
        //THe size will be used for the bounds of the scrollable image
        chatImageRect = activeChatImage.GetPixelAdjustedRect();
        if (chatImageRect.height < 1100)
        {
            canScroll = false;
            scrollbar.size = 1;
        }
        else
        {
            canScroll = true;
            topMax = activeChatImage.rectTransform.anchoredPosition.y;
            scrollbar.size = (chatImageRect.height - 1054) / 2646;
            scrollbar.size = 1 - scrollbar.size;
            scrollbar.value = 1;
            //chatHeightDiff = chatImageRect.y - (chatImageRect.height - botBoundMax);
            chatHeightDiff = chatImageRect.height - 1030;
        }
        lastCharacter = screen.character;
        profileButton.toCharacter = screen.character;
    }

    private void OnExitPage()
    {
        if (!canScroll)
        {
            return;
        }
        activeChatImage.rectTransform.anchoredPosition = new Vector3(0,topMax, 0);
    }
}
