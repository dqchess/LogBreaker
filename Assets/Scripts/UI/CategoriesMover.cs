using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ricimi;
using System;

public class CategoriesMover : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    GameObject m_prevButton;

    [SerializeField]
    GameObject m_nextButton;

    [SerializeField]
    RectTransform m_worldsRect;

    [SerializeField]
    Toggle[] m_toggles;

    [SerializeField]
    Text m_levelText;

    BackgroundChanger m_bgchanger;

    [Header("Variables")]

    [SerializeField]
    public float m_moveDistance;

    [SerializeField]
    float m_moveTime;

    [HideInInspector]
    public int m_NumOfPages;

    int m_pageIndes = 1;

    WaitForSeconds waitForAnim;

    bool m_canMove = true;

    void Awake()
    {
        m_bgchanger = FindObjectOfType<BackgroundChanger>();
        waitForAnim = new WaitForSeconds(m_moveTime + 0.1f);
        Initialize();
    }

    void Initialize()
    {
        m_NumOfPages = m_toggles.Length;
        SetWorld();
        SetLevelText(m_pageIndes);

        if (m_pageIndes == m_NumOfPages)
        {

            DisableNextLevelButton();
        }
        else if (m_pageIndes == 1)
        {
            DisablePrevLevelButton();
        }

        SetToggle(m_pageIndes - 1);
    }

    void SetWorld()
    {       
        int world = PlayerPrefs.GetInt("world");

        m_worldsRect.anchoredPosition -= Vector2.right * m_moveDistance * world;
        m_pageIndes += world;
   
        SetLevelText(m_pageIndes);
        m_bgchanger.SetBackground();
    }

    public void MoveRight()
    {
        if (!m_canMove || m_pageIndes >= m_NumOfPages)
            return;
        m_canMove = false;
        StartCoroutine(MoveRightCO());
    }

    public void MoveLeft()
    {
        if (!m_canMove || m_pageIndes <= 1)
            return;
        m_canMove = false;
        StartCoroutine(MoveLeftCO());       
    }

    void SetToggle(int _index)
    {
        for (int i = 0; i < m_toggles.Length; i++)
        {
            m_toggles[i].isOn = false;
        }
        m_toggles[_index].isOn = true;
        PlayerPrefs.SetInt("world", _index);
        m_bgchanger.SetBackground();
    }

    IEnumerator MoveRightCO()
    {
        LeanTween.move(m_worldsRect, m_worldsRect.anchoredPosition
            - Vector2.right * m_moveDistance, m_moveTime);
        m_pageIndes++;
        SetToggle(m_pageIndes - 1);
        if (m_pageIndes == m_NumOfPages)
        {
            DisableNextLevelButton();
        }
        if (m_pageIndes == 2)
        {
            EnablePrevLevelButton();
        }
        SetLevelText(m_pageIndes);
        yield return waitForAnim;
        m_canMove = true;
    }

    IEnumerator MoveLeftCO()
    {
        LeanTween.move(m_worldsRect, m_worldsRect.anchoredPosition
            + Vector2.right * m_moveDistance, m_moveTime);
        m_pageIndes--;
        SetToggle(m_pageIndes - 1);
        if (m_pageIndes == 1)
        {
            DisablePrevLevelButton();
        }
        if (m_pageIndes == m_NumOfPages - 1)
        {
            EnableNextLevelButton();
        }
        SetLevelText(m_pageIndes);
        yield return waitForAnim;
        m_canMove = true;
    }

    public void EnablePrevLevelButton()
    {
        var image = m_prevButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 1.0f;
        image.color = newColor;

        var shadow = m_prevButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 1.0f;
        shadow.color = newShadowColor;

        m_prevButton.GetComponent<Button>().interactable = true;
    }

    public void DisablePrevLevelButton()
    {
        var image = m_prevButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 40 / 255.0f;
        image.color = newColor;

        var shadow = m_prevButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 0.0f;
        shadow.color = newShadowColor;

        m_prevButton.GetComponent<Button>().interactable = false;
    }

    public void EnableNextLevelButton()
    {
        var image = m_nextButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 1.0f;
        image.color = newColor;

        var shadow = m_nextButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 1.0f;
        shadow.color = newShadowColor;

        m_nextButton.GetComponent<Button>().interactable = true;
    }

    public void DisableNextLevelButton()
    {
        var image = m_nextButton.GetComponentsInChildren<Image>()[1];
        var newColor = image.color;
        newColor.a = 40 / 255.0f;
        image.color = newColor;

        var shadow = m_nextButton.GetComponentsInChildren<Image>()[0];
        var newShadowColor = shadow.color;
        newShadowColor.a = 0.0f;
        shadow.color = newShadowColor;

        m_nextButton.GetComponent<Button>().interactable = false;
    }

    void SetLevelText(int _index)
    {
        m_levelText.text = _index.ToString();
    }
}

