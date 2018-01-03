using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ricimi;
using ExaGames.Common;

public class Exit : MonoBehaviour
{
    SceneTransition m_sceneTransition;

    void Awake()
    {
        m_sceneTransition = GetComponent<SceneTransition>();
    }

    public void Quit()
    {
        Application.Quit();
    }



}
