using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwingFromStart : MonoBehaviour
{
    [SerializeField] PendulumScript m_script;
    [SerializeField] GameObject m_targetObject;
    
    void Start()
    {
        if(m_script != null && m_targetObject != null)
        {
            m_script.StartSwing(m_targetObject.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
