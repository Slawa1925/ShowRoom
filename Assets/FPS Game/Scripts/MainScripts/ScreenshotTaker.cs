using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject screenshotCamera;
    public bool takeScreenshot;
    public Texture2D renderResult;
    public string path;

    public void OnPostRender()
    {
        if (takeScreenshot)
        {
            takeScreenshot = false;

            RenderTexture renderTexture = screenshotCamera.GetComponent<Camera>().targetTexture;

            renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            Debug.Log("ScreenshotTaken.");

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(path + "/Preview.png", byteArray);

            RenderTexture.ReleaseTemporary(renderTexture);
            screenshotCamera.GetComponent<Camera>().targetTexture = null;
        }
    }

    public void TakeScreenshot(string _path)
    {
        path = _path;

        screenshotCamera.transform.position = playerCamera.transform.position;
        screenshotCamera.transform.rotation = playerCamera.transform.rotation;

        screenshotCamera.GetComponent<Camera>().targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);

        takeScreenshot = true;
    }
}
