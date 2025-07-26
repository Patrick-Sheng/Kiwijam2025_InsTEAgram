using UnityEngine;

public class PauseManager : MonoBehaviour
{
  [SerializeField]
  private GameObject pausePanel;
  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      TogglePause();
    }
  }
    
  public void TogglePause()
  {
    if (pausePanel.activeSelf)
    {
      pausePanel.SetActive(false);
      Time.timeScale = 1f; // Resume the game
    }
    else
    {
      pausePanel.SetActive(true);
      Time.timeScale = 0f; // Pause the game
    }
  }
}
