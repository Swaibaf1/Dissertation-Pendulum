using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomPhysicsBase : MonoBehaviour
{
    protected Rigidbody m_rigidBody;

    [Header("RigidBody Component References")]

    [Tooltip("Mass -> Overrides RigidBody value")]
    [SerializeField] [Range(0f, 10f)] float m_mass;

    [Tooltip("Drag -> Overrides Rigidbody value")]
    [SerializeField] [Range(0f,0.05f)]float m_drag;

    [Tooltip("Checking this smooths effects of running physics between frames -> Overrides RigidBody value ")]
    [SerializeField] bool m_interpolate;

    [Header("Timestep Values")]

    [Space(5)]

    [Tooltip("Checking this sets Time.FixedDeltaTime to the custom timestep value below -> Affects FixedUpdate loop for entire project!")]
    [SerializeField] bool m_useCustomTimestep;

    [Tooltip("Changes Fixed Delta Time value if ^^ is checked. The lower the value, the more times the FixedUpdate loop runs in a second (default value = 0.02)")]
    [SerializeField] [Range(0.001f, 0.08f)] float m_customTimestepValue;


    public virtual void Awake()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
    }

    public virtual void SetVariables()
    {
        //sets base physics variables 
        m_rigidBody.mass = m_mass;
        m_rigidBody.drag = m_drag;
        if(m_interpolate) 
        {
            m_rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        }
        if (m_useCustomTimestep)
        {
            Time.fixedDeltaTime = m_customTimestepValue;
        }
    }
  

    #region getters and setters
    public float Mass { get { return m_mass; } set {  m_mass = value; }}
    public bool Interpolate { get { return m_interpolate; } set { m_interpolate = value; }}
    public float CustomTimeStepValue { get { return m_customTimestepValue; } set { m_customTimestepValue = value; }}
    public bool UseCustomTimeStep { get { return m_useCustomTimestep; } set { m_useCustomTimestep = value; }}
    public float Drag { get { return m_drag; } set { m_drag = value; }}



    #endregion
}
