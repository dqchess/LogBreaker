using UnityEngine;
using System.Collections;

public class DestoyByTime : MonoBehaviour
{
    [SerializeField]
    float timeToDestroy;

    void OnEnable()
    {
        Invoke("Destroy", timeToDestroy);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

}
