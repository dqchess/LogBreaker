using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateGPS : MonoBehaviour {

    [SerializeField]
    Text m_loginText;
 

    void Start()
    {
        if (!GameControl.control.m_IsLoggedIn)
        {
            GPSManager.Activate();           
        }
    }

    void Update()
    {
        m_loginText.text = GameControl.control.m_IsLoggedIn.ToString();
    }
}
