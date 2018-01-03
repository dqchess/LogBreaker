using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using System;

public class GPSManager {

    public static void Activate()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                //Debug.Log("GPS autenticated " + success);
                GameControl.control.m_IsLoggedIn = success;
            });
        }
        
        //Debug.Log(Social.localUser.authenticated);

    }

    public static bool IsAutenticated()
    {
        return Social.localUser.authenticated;
    }

    public static void OperLeaderBoardsUI()
    {
        Social.ShowLeaderboardUI();
    }

    public static void OpenAchievmentsUI()
    {
        Social.ShowAchievementsUI();
    }

    public static void SetRecoredForLeaderboard(WorldState state)
    {
        int totalScore = 0;

        int[] scores = state.scores;

        for (int i = 0; i < 10; i++)
        {

            totalScore += scores[i];
            
        }
        switch (state.worldNum)
        {
            case 0:
                Social.ReportScore(totalScore, GPGSIds.leaderboard_world_1, (bool success) => 
                {
                    if (success)
                    {
                        //Debug.Log("success");
                    }
                    else
                    {
                       // Debug.Log("fail");
                    }
                }
                );
                break;
            case 1:
                Social.ReportScore(totalScore, GPGSIds.leaderboard_world_2, (bool success) =>
                {
                    if (success)
                    {
                       // Debug.Log("success");
                    }
                    else
                    {
                       // Debug.Log("fail");
                    }
                }
                                ); break;
            case 2:
                Social.ReportScore(totalScore, GPGSIds.leaderboard_world_3, (bool success) =>
                {
                    if (success)
                    {
                       // Debug.Log("success");
                    }
                    else
                    {
                       // Debug.Log("fail");
                    }
                }
                                ); break;
            default:
                break;


        }
       
    }

    public static void CheckForAchievments(WorldState state)
    {
        if(CheckXstarAchievmant(state, 1))
        {
            Report1StarAchievment(state.worldNum + 1);
        }
        if (CheckXstarAchievmant(state, 2))
        {
            Report2StarAchievment(state.worldNum + 1);
        }
        if (CheckXstarAchievmant(state, 3))
        {
            Report3StarAchievment(state.worldNum + 1);
        }
    }

    static void Report3StarAchievment(int worldNum)
    {
        //Debug.Log("Achievment 3 star, world " + worldNum);
        switch (worldNum)
        {
            case 1:
                Social.ReportProgress(GPGSIds.achievement_world_1_completed_with_3_stars_on_each_level, 100d, (bool success) => { });
                break;
            case 2:
                Social.ReportProgress(GPGSIds.achievement_world_2_completed_with_3_stars_on_each_level, 100d, (bool success) => { });
                break;
            case 3:
                Social.ReportProgress(GPGSIds.achievement_world_3_completed_with_3_stars_on_each_level, 100d, (bool success) => { });
                break;
            default:
                break;
        }
    }

    static void Report2StarAchievment(int worldNum)
    {
       // Debug.Log("Achievment 2 star, world " + worldNum);
        switch (worldNum)
        {
            case 1:
                Social.ReportProgress(GPGSIds.achievement_world_1_completed_with_minimum_of_2_stars_on_each_level, 100d, (bool success) => { });
                break;
            case 2:
                Social.ReportProgress(GPGSIds.achievement_world_2_completed_with_minimum_of_2_stars_on_each_level, 100d, (bool success) => { });
                break;
            case 3:
                Social.ReportProgress(GPGSIds.achievement_world_3_completed_with_minimum_of_2_stars_on_each_level, 100d, (bool success) => { });
                break;
            default:
                break;
        }
    }

    static void Report1StarAchievment(int worldNum)
    {
        //Debug.Log("Achievment 1 star, world " + worldNum);
        switch (worldNum)
        {
            case 1:
                Social.ReportProgress(GPGSIds.achievement_world_1_completed, 100d, (bool success) => { });
                break;
            case 2:
                Social.ReportProgress(GPGSIds.achievement_world_2_completed, 100d, (bool success) => { });
                break;
            case 3:
                Social.ReportProgress(GPGSIds.achievement_world_3_completed, 100d, (bool success) => { });
                break;
            default:
                break;
        }
    }

    static bool CheckXstarAchievmant(WorldState state,  int numberOfStars)
    {
        for (int i = 0; i < state.scores.Length; i++)
        {
            int starValuesIndex = state.worldNum * 10 + i;
            StarsValues values = GameControl.control.m_LevelsStarValuesSO.LevelsStarValues[starValuesIndex];
            int stars = GameControl.control.GetStars(state.scores[i], GameControl.control.m_StarsValues);
            Debug.Log(stars);
            if (stars < numberOfStars)
            {
                return false;
            }
        }
        return true;
    }
}
