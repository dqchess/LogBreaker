using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    [SerializeField]
    ParticleSystem m_effect;

    AudioSource m_audio;

    bool m_isShooting;

    WaitForSeconds WFSShootDuration;
    
    void Awake()
    {
        m_audio = GetComponent<AudioSource>();
    }
  
    public void Shoot(float _duration)
    {
        StopAllCoroutines();
        m_audio.PlayOneShot(m_audio.clip);
        WFSShootDuration = new WaitForSeconds(_duration);     
        StartCoroutine(ShootCO());      
    }

    IEnumerator ShootCO()
    {
        m_effect.Play();
        m_isShooting = true;
        yield return WFSShootDuration;
        m_effect.Stop();
        m_isShooting = false;
    }
}
