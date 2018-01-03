using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;

[System.Serializable]
public class TutorialInfo
{
    public int worldNum;
    public int levelNum;
    public GameObject popup;
}

public class TurorialManager : MonoBehaviour {

    [SerializeField]
	public List<TutorialInfo> m_tutorialInfoList;

    PopupOpener m_popupOpener;

    GameManager m_gameManager;

    int m_activeLevelNum;

    int m_activeWorldNum;

    void Awake()
    {
        GetReferences();
        m_activeLevelNum = GameControl.control.m_ActiveLevelNum;
        m_activeWorldNum = GameControl.control.m_ActiveWorldNum;
    }

    void GetReferences()
    {
        m_popupOpener = GetComponent<PopupOpener>();
        m_gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        for (int i = 0; i < m_tutorialInfoList.Count; i++)
        {
            if(m_tutorialInfoList[i].worldNum == m_activeWorldNum && m_tutorialInfoList[i].levelNum == m_activeLevelNum)
            {
                if(GameControl.control.m_ActiveLevelScore <= 1)
                {
                    m_popupOpener.popupPrefab = m_tutorialInfoList[i].popup;
                    m_popupOpener.OpenPopup();
                    return;
                }              
            }           
        }
        m_gameManager.Initialize();
    }   
}
