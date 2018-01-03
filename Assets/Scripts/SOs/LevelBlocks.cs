using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelBlockData
{
    public string Name;
    public GameObject Prefab;
}


[CreateAssetMenu]
public class LevelBlocks : ScriptableObject {

    public List<LevelBlockData> Blocks = new List<LevelBlockData>();
}


