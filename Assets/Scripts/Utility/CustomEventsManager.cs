using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public static class CustomEventsManager{

    public static void LevelStartedEvent(int levelNumber)
    {
        //Debug.Log("Level started " + levelNumber);
        Analytics.CustomEvent("LevelStarted", new Dictionary<string, object>
        {
            { "levelNumber", levelNumber },
        }     
        );
    }

    public static void LevelCompleteEvent(int levelNumber ,int timeOrMoves, int livesLeft, int score, int numOfStars)
    {
        //Debug.Log("win event");
        Analytics.CustomEvent("LevelCompleted", new Dictionary<string, object>
        {
            { "levelNumber", levelNumber },
            { "timeMoves", timeOrMoves },
            { "lives", livesLeft },
        }
        );
    }

    public static void LevelOverEvent(int levelNumber, int timeOrMoves, int livesLeft)
    {
        //Debug.Log("Lose event");
        Analytics.CustomEvent("GameOver", new Dictionary<string, object>
        {
            { "levelNumber", levelNumber },
            { "timeMoves", timeOrMoves },
            { "lives", livesLeft },
        }
       );
    }


    public static void BoughtGameOverBoosterEvent()
    {
        //Debug.Log("Bought GameOver Booster");
        Analytics.CustomEvent("BoughtGameOverBooster");
    }



    public static void BoughtShopBoosterEvent(POWERUP powerup)
    {
       // Debug.Log("Bought shop Booster");
        Analytics.CustomEvent("BoughtShopBooster", new Dictionary<string, object>
        {
            { "power up", powerup},
        }
        );
    }

    public static void CanDoublePoints()
    {
        //Debug.Log("Can double points");
        Analytics.CustomEvent("BoughtGameOverBooster");
    }

    public static void DoubledPoints()
    {
        //Debug.Log("Doubled points");
        Analytics.CustomEvent("BoughtGameOverBooster");
    }


}
