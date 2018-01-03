using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TexteureFromScreen : MonoBehaviour
{

    [Header("References")]

    [SerializeField]
    int Width;

    [SerializeField]
    int Height;

    [SerializeField]
    Image BackgroundImg;
    
    void Start()
    {
        StartCoroutine(TakeSnapshot(Screen.width, Screen.height));
    }

    public IEnumerator TakeSnapshot(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot("Assets/BackgroundImg.png");       
    }
}
