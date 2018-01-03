using UnityEngine;
using UnityEngine.UI;

public class PlayPopup : MonoBehaviour
{
    public Color enabledColor;
    public Color disabledColor;
    public Text ScoreText;
    public Text LevelNumText;

    public Image leftStarImage;
    public Image middleStarImage;
    public Image rightStarImage;

    private void Start()
    {
        int stars = GameControl.control.m_ActiveLevelStars;
        SetAchievedStars(stars);
        LevelNumText.text = (GameControl.control.m_LevelIndex).ToString();
    }

    public void SetAchievedStars(int starsObtained)
    {
        if (starsObtained == 0)
        {
            leftStarImage.color = disabledColor;
            middleStarImage.color = disabledColor;
            rightStarImage.color = disabledColor;
        }
        else if (starsObtained == 1)
        {
            leftStarImage.color = enabledColor;
            middleStarImage.color = disabledColor;
            rightStarImage.color = disabledColor;
        }
        else if (starsObtained == 2)
        {
            leftStarImage.color = enabledColor;
            middleStarImage.color = enabledColor;
            rightStarImage.color = disabledColor;
        }
        else if (starsObtained == 3)
        {
            leftStarImage.color = enabledColor;
            middleStarImage.color = enabledColor;
            rightStarImage.color = enabledColor;
        }
        int scoreText = GameControl.control.m_ActiveLevelScore;
        if(scoreText == 1)
        {
            scoreText -= 1;
        }

        ScoreText.text = scoreText.ToString();
    }
}

