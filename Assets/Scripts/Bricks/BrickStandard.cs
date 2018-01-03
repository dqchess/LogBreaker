using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickStandard : Brick {

    [Header("References")]

    [SerializeField]
    Mesh[] DamagedMeshes;

    [SerializeField]
    GameObject ExplosionEffect;

    [SerializeField]
    GameObject CrackEffect;

    MeshFilter m_meshFilter;

    Material m_material;

    int m_meshIndex;

    protected override void Awake()
    {
        m_meshIndex = DamagedMeshes.Length - 1;
        base.Awake();
        GetReferences();
    }

    void GetReferences()
    {
        m_meshFilter = GetComponentInChildren<MeshFilter>();
        m_material = GetComponentInChildren<MeshRenderer>().material;
    }

    public override void HandleBallCollision(bool _ballCollision, GameObject _collidingBall = null)
    {
        base.HandleBallCollision(_ballCollision);
                     
        if (HitPoints > 0)
        {
            Instantiate(CrackEffect, transform.position, transform.rotation);
            m_meshFilter.mesh = DamagedMeshes[m_meshIndex];
            m_meshIndex--;           
            m_material.color = m_material.color * 0.8f;
        }         
    }

    protected override void DestroyBrick(GameObject collidingBall = null)
    {
        Instantiate(ExplosionEffect, transform.position, transform.rotation);
        base.DestroyBrick();
    }
}
