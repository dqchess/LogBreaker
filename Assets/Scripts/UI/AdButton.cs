using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

public class SpinButton : MonoBehaviour {

    PopupOpener m_popupOpener;

    void Awake()
    {
        m_popupOpener = GetComponent<PopupOpener>();
    }

    public void SpinWheel()
    {
        if (AdManager.Instance.m_IsPlayingAd)
            return;
        AdManager.Instance.m_AdReward = AD_REWARD.LIVE;
        AdManager.Instance.ShowAd();
    }
}
