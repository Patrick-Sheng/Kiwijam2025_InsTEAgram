using UnityEngine;

public class IntroManager : MonoBehaviour
{
  [SerializeField]
  private GameObject introPanel;


  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    introPanel.SetActive(true);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      introPanel.SetActive(false);
    }
  }
}
