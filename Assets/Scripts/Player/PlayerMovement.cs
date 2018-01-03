using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    Image m_leftArrowImage;

    [SerializeField]
    Image m_rightArrowImage;

    Transform m_transform;

    Rigidbody2D m_RB2D;

    CapsuleCollider2D m_capsuleCollider;

    [Header("Variables")]

    [SerializeField]
    Color m_pressedColor;

    [SerializeField]
    Color m_normalColor;

    [HideInInspector]
    public bool m_IsReversedDirection;

    [HideInInspector]
    public bool m_CanMove;

    float m_speed;

    bool m_isMovingLeft;

    bool m_isMovingRight;


    void Awake()
    {
        GetReferences();
        m_CanMove = true;
        m_speed = GameControl.control.m_LevelInfo.paddleSpeed;
    }

    void GetReferences()
    {
        m_capsuleCollider = GetComponent<CapsuleCollider2D>();
        m_transform = GetComponent<Transform>();
        m_RB2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        Input.multiTouchEnabled = false;
        m_leftArrowImage.color = m_normalColor;
        m_rightArrowImage.color = m_normalColor;
        transform.localScale *= GameControl.control.m_LevelInfo.paddleScale;
    }

    public bool CheckIfInBounds(Vector3 point)
    {
        if (m_capsuleCollider.bounds.Contains(point))
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if(m_transform.position.x > 15.5f || m_transform.position.x < -13.5f)
        {
            float x = Mathf.Clamp(m_transform.position.x, -13.5f, 15.5f);
            m_transform.position = new Vector3(x, m_transform.position.y, 0f);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            StopLeft();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StopRight();
        }
    }

    public void MoveLeft()
    {     
        if (!m_CanMove)
            return;

        m_isMovingLeft = true;
        Vector2 direction = (m_IsReversedDirection) ? Vector2.right : Vector2.left;
        m_RB2D.velocity = direction * m_speed;
        ColorArrows();
    }

    public void MoveRight()
    {
        if (!m_CanMove)
            return;

        m_isMovingRight = true;
        Vector2 direction = (!m_IsReversedDirection) ? Vector2.right : Vector2.left;
        m_RB2D.velocity = direction * m_speed;
        ColorArrows();
    }

    public void StopLeft()
    {
        m_isMovingLeft = false;
        if (m_isMovingRight)
        {
            MoveRight();
        }
        else
        {
            m_RB2D.velocity = Vector3.zero;
        }
        ColorArrows();
    }

    public void StopRight()
    {
        m_isMovingRight = false;
        if (m_isMovingLeft)
        {
            MoveLeft();
        }
        else
        {
            m_RB2D.velocity = Vector3.zero;
        }
        ColorArrows();
    }

    void ColorArrows()
    {
        if (m_RB2D.velocity.x == 0)
        {
            m_leftArrowImage.color = m_normalColor;
            m_rightArrowImage.color = m_normalColor;
        }
        else if(m_RB2D.velocity.x > 0)
        {
            if (!m_IsReversedDirection)
            {
                m_leftArrowImage.color = m_normalColor;
                m_rightArrowImage.color = m_pressedColor;
            }
            else
            {
                m_leftArrowImage.color = m_pressedColor;
                m_rightArrowImage.color = m_normalColor;
            
             }
            
        }
        else
        {
            if (!m_IsReversedDirection)
            {
                m_leftArrowImage.color = m_pressedColor;
                m_rightArrowImage.color = m_normalColor;
            }
            else
            {
                m_leftArrowImage.color = m_normalColor;
                m_rightArrowImage.color = m_pressedColor;

            }
        }
    }
}