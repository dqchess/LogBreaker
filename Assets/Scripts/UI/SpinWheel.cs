// Copyright (C) 2015, 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Ricimi
{

    public enum SPIN_TYPE
    {
        DAILY,
        DEATH
    }
    // This class manages the rotation of the example spin wheel in the demo.
    public class SpinWheel : MonoBehaviour
    {
        // This animation curve drives the spin wheel motion.
        public AnimationCurve AnimationCurve;

        private PopupOpener m_popupOpener;

        public GameObject DailySpinPopup,
            DeathSpinPopup;

        public SPIN_TYPE spinType;

        public Transform[] itemsArray;

        public ParticleSystem Effect;

        private bool m_spinning = false;
     
        private void Awake()
        {          
            m_popupOpener = GetComponent<PopupOpener>();
         
        }

        public void Spin()
        {
            if (!m_spinning)
                StartCoroutine(DoSpin());
        }

        private IEnumerator DoSpin()
        {
            m_spinning = true;
            var timer = 0.0f;
            var startAngle = transform.eulerAngles.z;

            var time = Random.Range(3f, 7f);
            var maxAngle = Random.Range(360, 1080);

            while (timer < time)
            {               
                var angle = AnimationCurve.Evaluate(timer / time) * maxAngle;
                transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if(spinType == SPIN_TYPE.DAILY)
            {
                
            }
            else if(spinType == SPIN_TYPE.DEATH)
            {
                GetLife();
            }
            Effect.Play();          
            transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
            m_spinning = false;
        }

        void GetBooster()
        {
            Transform booster = itemsArray[0];

            for (int i = 1; i < itemsArray.Length; i++)
            {
                if(itemsArray[i].position.y > booster.position.y)
                {
                    booster = itemsArray[i];
                }
            }          
            m_popupOpener.popupPrefab = DailySpinPopup;
            m_popupOpener.OpenPopup();
            
            transform.parent.GetComponent<Popup>().Close();
                               
            PlayerPrefs.SetString("WheelDate", DateTime.Now.ToBinary().ToString());
        }

        void GetLife()
        {
            Transform award = itemsArray[0];

            for (int i = 1; i < itemsArray.Length; i++)
            {
                if (itemsArray[i].position.y > award.position.y)
                {
                    award = itemsArray[i];
                }
            }

            transform.parent.GetComponent<Popup>().Close();
            
            if (award.gameObject.name == "video")
            {
                if (AdManager.Instance.m_IsPlayingAd)
                    return;
                AdManager.Instance.m_AdReward = AD_REWARD.LIVE;
                
                AdManager.Instance.ShowAd();
            }
            else
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                
                gameManager.GameOver();
            }      
        }
    }
}
