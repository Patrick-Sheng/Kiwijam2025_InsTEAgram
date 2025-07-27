using UnityEngine;
using System.IO;

public class UIScreenshot : MonoBehaviour
{
    public KeyCode captureKey = KeyCode.F12;

    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            StartCoroutine(CaptureScreenshot());
        }
    }

    public void CaptureScreenshotButton()
    {
        StartCoroutine(CaptureScreenshot());
    }

  System.Collections.IEnumerator CaptureScreenshot()
  {
    yield return new WaitForEndOfFrame();

    int width = UnityEngine.Screen.width;
    int height = UnityEngine.Screen.height;

    Texture2D screenImage = new Texture2D(width, height, TextureFormat.RGB24, false);
    screenImage.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    screenImage.Apply();

    byte[] imageBytes = screenImage.EncodeToPNG();
    string folderPath = Path.Combine(Application.dataPath, "Screenshots");
    if (!Directory.Exists(folderPath))
    {
      Directory.CreateDirectory(folderPath);
    }

    string fileName = "ui_screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
    string path = Path.Combine(folderPath, fileName);

    File.WriteAllBytes(path, imageBytes);
    Debug.Log("Saved UI screenshot to: " + path);

    Destroy(screenImage);
        
        GameStateManager.Instance.ChangeState(GameStateManager.GAME_STATES.GAME_OVER);
    }
}
