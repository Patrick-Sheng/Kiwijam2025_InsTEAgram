using UnityEngine;

public class GameStateManager : MonoBehaviour
{


  private static GameStateManager _instance;
  public static GameStateManager Instance { get { return _instance; } }

  [SerializeField]
  private GameObject introPanel;
  [SerializeField]
  private GameObject pausePanel;
  [SerializeField]
  private GameObject gameOverPanel;


  public enum GAME_STATES
  {
    INTRO,
    PLAYING,
    PAUSED,
    GAME_OVER
  }
  private GAME_STATES currentState;


  void Start()
  {
    if (_instance != null && _instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      _instance = this;
      currentState = GAME_STATES.INTRO;
      introPanel.SetActive(true);
    }
  }

  // Update is called once per frame
  void Update()
  {
    switch (currentState)
    {
      case GAME_STATES.INTRO:
        if (Input.GetKeyDown(KeyCode.Space))
        {
          ChangeState(GAME_STATES.PLAYING);
        }
        break;

      case GAME_STATES.PLAYING:
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          ChangeState(GAME_STATES.PAUSED);
        }
        break;

      case GAME_STATES.PAUSED:
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          ChangeState(GAME_STATES.PLAYING);
        }
        break;

      case GAME_STATES.GAME_OVER:
        break;
      default:
        Debug.LogWarning("Unhandled game state: " + currentState);
        break;
    }
  }

  public void ChangeState(GAME_STATES newState)
  {
    currentState = newState;
    Debug.Log("Game state changed to: " + currentState);

    // Additional logic can be added here for each state change if needed
    switch (currentState)
    {
      case GAME_STATES.INTRO:
        introPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        break;
      case GAME_STATES.PLAYING:
        introPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        break;
      case GAME_STATES.PAUSED:
        introPanel.SetActive(false);
        pausePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        break;
      case GAME_STATES.GAME_OVER:
        introPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        break;
    }
  }

  // Following methods are used for user clicking buttons in the UI.
  // The ones above are for handling keyboard input.
  public void ToggleStartGame()
  {
    if (currentState != GAME_STATES.INTRO)
    {
      Debug.LogWarning("Game can only be started from the INTRO state.");
      return;
    }
    ChangeState(GAME_STATES.PLAYING);
    introPanel.SetActive(false);
    pausePanel.SetActive(false);
    gameOverPanel.SetActive(false);
  }

  public void TogglePause()
  {
    if (currentState == GAME_STATES.PAUSED)
    {
      ChangeState(GAME_STATES.PLAYING);
    }
    else if (currentState == GAME_STATES.PLAYING)
    {
      ChangeState(GAME_STATES.PAUSED);
    }
  }

  public void ToggleGameOver()
  {
    ChangeState(GAME_STATES.GAME_OVER);
    introPanel.SetActive(false);
    pausePanel.SetActive(false);
    gameOverPanel.SetActive(true);
  }

  public void ToggleRestartGame()
  {
    ChangeState(GAME_STATES.INTRO);
    introPanel.SetActive(true);
    pausePanel.SetActive(false);
    gameOverPanel.SetActive(false);
  }
}
