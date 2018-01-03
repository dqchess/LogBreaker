using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using Ricimi;

public class BackgroundChanger : MonoBehaviour {

    Image m_image;

    void Awake()
    {
        m_image = GetComponent<Image>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("world"))
        {           
            int i = PlayerPrefs.GetInt("world");           
            SetStaff(i);
        }
    }

    public void SetBackground()
    {
        if (PlayerPrefs.HasKey("world"))
        {
            int i = PlayerPrefs.GetInt("world");
            BackgroundMusic.Instance.ChangeBGmusic(i);
            StartCoroutine(ChangeImageCO(i));
        }
    }

    IEnumerator ChangeImageCO(int _index)
    {
        m_image.CrossFadeAlpha(.5f, .15f, true);
        yield return new WaitForSeconds(.2f);
        SetStaff(_index);   
        m_image.CrossFadeAlpha(1f, .15f, true);
    }

    void SetStaff(int _index)
    {
        m_image.sprite = GameControl.control.Backgrounds[_index];      
    }
}
