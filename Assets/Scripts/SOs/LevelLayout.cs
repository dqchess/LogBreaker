using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class LevelElement
{
    public GameObject Prefab;
    public Vector3 Position;
}


[CreateAssetMenu]
public class LevelLayout : ScriptableObject
{
    public List<LevelElement> levelLayout = new List<LevelElement>();
}
