using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsFiller : MonoBehaviour {

    [Header("References")]

    StarsValues m_starValues;

    [SerializeField]
    ParticleSystem m_effect1;

    [SerializeField]
    ParticleSystem m_effect2;

    [SerializeField]
    Image m_star1;

    [SerializeField]
    Image m_star2;

    [SerializeField]
    Image m_star3;

    [Header("Variables")]

    [SerializeField]
    Vector3 m_pos1;

    [SerializeField]
    Vector3 m_pos2;

    int m_starsObtained;

    void Awake()
    {
        m_starValues = GameControl.control.m_StarsValues;
        m_starsObtained = 0;
    }

    public void FillStars(int _score)
    {     
        if(m_starsObtained == 0)
        {
            m_star1.fillAmount = (float)_score / (float)m_starValues.Star1;

            if (m_star1.fillAmount >= 1)
            {
                m_starsObtained++;
               // PlayEffects();
            }               
        }
             
        if (m_starsObtained == 1)
        {
            m_star2.fillAmount = (float)(_score - m_starValues.Star1) / 
                (float)(m_starValues.Star2 - m_starValues.Star1);

            if (m_star2.fillAmount >= 1)
            {
                m_starsObtained++;
               // PlayEffects();
            }
        }
        if (m_starsObtained == 2)
        {
            m_star3.fillAmount = (float)(_score - m_starValues.Star2) / 
                (float)(m_starValues.Star3 - m_starValues.Star2);

            if (m_star3.fillAmount >= 1)
            {
                m_starsObtained++;
                //PlayEffects();
            }
        }        
    }

    void PlayEffects()
    {
        m_effect2.Play();
        m_effect1.Play();
    }

    public void PlayEffectAtPoition()
    {
        m_effect1.transform.position = m_pos1;
        m_effect2.transform.position = m_pos2;
        PlayEffects();
    }
}
