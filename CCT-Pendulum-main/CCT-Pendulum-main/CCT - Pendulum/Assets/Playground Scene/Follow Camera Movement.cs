using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCameraMovement : MonoBehaviour
{

    Camera m_camera;
    [SerializeField] GameObject m_gameObjectToFollow;
    [SerializeField] Vector3 m_cameraPositionOffset;


    Vector2 m_cameraPitchYaw;


    // Start is called before the first frame update
    void Awake()
    {
        m_camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_camera != null && m_gameObjectToFollow != null)
        {
            m_camera.transform.position = m_gameObjectToFollow.transform.position + m_cameraPositionOffset;
        }


    }
}
