using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public float Width = 1f;
    public float Height = .5f;

    private void OnDrawGizmos()
    {
        Vector3 pos = Camera.main.transform.position;

        for (float y = pos.y - 80.0f; y < pos.y + 80f; y += Height)
        {
            Gizmos.DrawLine(new Vector3(-20f, Mathf.Floor(y / Height) * Height, 0f),
                            new Vector3(20f, Mathf.Floor(y / Height) * Height, 0f));
        }

        for (float x = pos.x - 120.0f; x < pos.x + 120.0f; x += Width)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / Width) * Width, -20f, 0.0f),
                            new Vector3(Mathf.Floor(x / Width) * Width, 20f, 0.0f));
        }
    }
}
