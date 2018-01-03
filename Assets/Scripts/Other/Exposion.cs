using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EXPLOSION_TYPE
{
    Wood,
    Rock,
    Log
}

public class Exposion : MonoBehaviour
{
    [SerializeField]
    EXPLOSION_TYPE m_explosionType;

    List<GameObject> m_meshes;

    ExplodeMesh m_explosionMesh;

    void Awake()
    {
        m_explosionMesh = GameObject.FindObjectOfType<ExplodeMesh>();
    }

    public void Explode()
    {              
        for (int i = 0; i < m_meshes.Count; i++)
        {
            m_meshes[i].transform.position = transform.position;
            m_meshes[i].transform.rotation = transform.rotation;
            Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
            m_meshes[i].AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), explosionPos, 5);
            Destroy(m_meshes[i], 5 + Random.Range(0.0f, 5.0f));
        }            
        Destroy(gameObject);       
    }
}
