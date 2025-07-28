using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

public class CharacterControllerInterface : MonoBehaviour
{
    [Header("References")]

    [SerializeField] PendulumScript m_script;
    [SerializeField] LayerMask m_targetLayer;
    [SerializeField] LayerMask m_worldObjectLayer;


    [Header("Movement Variables")]
    
    [SerializeField] float m_speed;
    float m_speedLocal;

    [SerializeField] float m_jumpForce;
    bool m_canJump;
    [SerializeField] float m_extraGravity;
    
    [Tooltip("1 = full control, 0 = no control")]
    [SerializeField] [Range(0,1)] float m_airControl;

    [Header("Misc")]

    [SerializeField] float m_respawnThreshold;
    float m_horizontalMoveForce;
    float m_verticalMoveForce;
    Vector3 m_startPosition;

 
    private void Start()
    {
        m_startPosition = m_script.transform.position;
    }

    public void Update()
    {
        m_script.RigidBody.velocity += new Vector3(m_horizontalMoveForce, 0, m_verticalMoveForce);

        JumpCheck();

        AirControlCheck();

        RespawnCheck();

    }

    void Jump()
    {
        m_script.RigidBody.velocity += new Vector3(0, m_jumpForce, 0);
    }

    void AirControlCheck()
    {
        if (m_script.IsSwinging)
        {
            m_speedLocal = m_speed * m_airControl;
        }
        else
        {
            m_speedLocal = m_speed;
        }
    }

    void JumpCheck()
    {
        if (Physics.Raycast(m_script.RigidBody.transform.position, Vector3.down, 0.8f, m_worldObjectLayer) || Physics.Raycast(m_script.RigidBody.transform.position, Vector3.down, 0.8f, m_targetLayer))
        {
            m_canJump = true;
        }
        else
        {
            m_canJump = false;

            if (!m_script.IsSwinging)
            {
                m_script.RigidBody.velocity -= new Vector3(0, m_extraGravity, 0);
            }
        }
    }
    
    void RespawnCheck()
    {
        if (m_script.transform.position.y <= m_respawnThreshold)
        {
            m_script.transform.SetPositionAndRotation(m_startPosition, Quaternion.identity);
            m_script.RigidBody.velocity = Vector3.zero;
        }
    }

    public void OnClickPressed(InputAction.CallbackContext _context)
    {
        if(_context.started)
        {
            Vector3 _mousePosition = Input.mousePosition;
            
            Ray _ray = Camera.main.ScreenPointToRay(_mousePosition);

            if(Physics.Raycast(_ray, out RaycastHit _hitData, Mathf.Infinity, m_targetLayer))
            {
                if(_hitData.collider != null)
                {
                    m_script.StartSwing(_hitData.point);
                }
            }
            else
            {
                print("NO TARGET");
            }

        }    

        if(_context.canceled) 
        {
            m_script.EndSwing();
        }

    }

    public void OnPlayerMoveHorizontal(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            m_horizontalMoveForce = _context.ReadValue<float>() * m_speedLocal;
        }

        if (_context.canceled)
        {
            m_horizontalMoveForce = 0;
        }
    }

    public void OnPlayerMoveVertical(InputAction.CallbackContext _context)
    {
        if (_context.started)
        {
            m_verticalMoveForce = _context.ReadValue<float>() * m_speedLocal;
        }

        if (_context.canceled)
        {
            m_verticalMoveForce = 0;
        }


    }

    public void OnJumpPressed(InputAction.CallbackContext _context)
    {
        if(_context.started && m_canJump)
        {
            Jump();
        }
    }

}
