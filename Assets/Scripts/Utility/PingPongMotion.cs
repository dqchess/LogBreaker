using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMotion : MonoBehaviour {

    [Header("Variables")]

    [SerializeField]
    Vector3 MoveDir;

    [SerializeField]
    float Speed;

    [SerializeField]
    float TravelDistance;

    [SerializeField]
    float rotationSpeed;

    private Transform ThisTransform;  

    IEnumerator Start()
    {
        ThisTransform = transform;
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        while (true)
        {
            MoveDir = MoveDir * -1;
            yield return StartCoroutine(Travel());
        }
    }

    IEnumerator Travel()
    {
        float DistanceTravelled = 0;
        while (DistanceTravelled < TravelDistance)
        {
            Vector3 DistToTravel = MoveDir * Speed * Time.deltaTime;
            ThisTransform.position += DistToTravel;
            DistanceTravelled += DistToTravel.magnitude;
            ThisTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
