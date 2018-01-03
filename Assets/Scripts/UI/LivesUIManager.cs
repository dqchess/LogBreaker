using UnityEngine;
using ExaGames.Common;
using UnityEngine.UI;

public class LivesUIManager : MonoBehaviour
{
    [SerializeField]
    private float counter;

    private bool canChange = true;

    public LivesManager LivesManager;

    public Text LivesText;

    public Text TimeToNextLifeText;

    public void OnLivesChanged()
    {
        LivesText.text = LivesManager.LivesText;
    }

    /// <summary>
    /// Time to next life changed event handler, changes the label value.
    /// </summary>
    public void OnTimeToNextLifeChanged()
    {
        if (canChange)
        {
            TimeToNextLifeText.text = LivesManager.RemainingTimeString;
            counter = 0;
            canChange = false;
        }
        
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 1)
        {
            canChange = true;
        }
    }
}
