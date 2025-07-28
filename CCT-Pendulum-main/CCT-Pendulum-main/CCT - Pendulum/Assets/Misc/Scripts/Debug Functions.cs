using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TestFunctions : MonoBehaviour
{
    bool m_ispaused = false;
  
    PendulumScript m_pendulumScript;
    public void OnQuitPressed(InputAction.CallbackContext _context)
    {
        if(_context.started)
        {
            Application.Quit();
        }
    }

    public void OnPauseToggle(InputAction.CallbackContext _context)
    {
        if(_context.started)
        {
            if(!m_ispaused)
            {
                Time.timeScale = 0;
                m_ispaused = true;
            }
            else
            {
                Time.timeScale = 1;
                m_ispaused = false;
            }
        }


    }

    public void OnRestartPressed(int _scene)
    {
      
            SceneManager.LoadScene(_scene);
        
    }

    public void LoadNewScene(int _scene)
    {
        SceneManager.LoadScene(_scene);
    }

}


