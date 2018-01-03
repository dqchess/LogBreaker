using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovingBrick
{
    public BezierSpline spline;
    public GameObject brick;
    public float duration;
    public SplineWalkerMode mode;
}

[CreateAssetMenu]
public class LevelMovers : ScriptableObject {

    public List<MovingBrick> movingBricksList = new List<MovingBrick>();
}
