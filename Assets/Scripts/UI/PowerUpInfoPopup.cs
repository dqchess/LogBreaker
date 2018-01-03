using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpInfoPopup : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    Text m_descriptionText;

    [SerializeField]
    Image m_powerUpImage;

    [SerializeField]
    Text m_headlineText;

    public void Initialize(string _name, string _description, Sprite _sprite)
    {
        m_headlineText.text = _name;
        m_descriptionText.text = _description;
        m_powerUpImage.sprite = _sprite;
    }
}
