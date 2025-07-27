using System;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
  [Header("Panels")]
  public GameObject currentPanel; 
  public GameObject nextPanel;

  public void SwitchToNextPanel()
  {
    Console.WriteLine("Switching panels...");
    if (currentPanel != null)
    {
      currentPanel.SetActive(false);
    }

    if (nextPanel != null)
    {
      nextPanel.SetActive(true);
    }
  }
}
