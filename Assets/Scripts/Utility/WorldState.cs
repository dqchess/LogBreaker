using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum POWERUP
{
    BOMB,
    BALL,
    LIFE,
    LIFE_NORMAL,
    SHOOT,
    FULL_LIVES,
    BOMB_BOOSTER,
    DESTROY_BOOSTER
}

[System.Serializable]
public class WorldState
{   
    public int[] scores;
    public int worldNum;
    
    public WorldState(int[] scores, int worldNum)
    {
        this.scores = scores;
        this.worldNum = worldNum;
    }
}


[System.Serializable]
public class PlayerData
{ 
    public List<WorldState> worldStatesList;
    public int Coins;
    public int activeWorldsCount;
    public List<POWERUP> inventory;

}
