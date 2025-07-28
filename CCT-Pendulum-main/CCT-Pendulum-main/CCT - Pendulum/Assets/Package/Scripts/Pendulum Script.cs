using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(LineRenderer))]

public class PendulumScript : CustomPhysicsBase
{
    #region inspector variables

    [Header("Pendulum Specific Values")]

    [Space(10)]

    [Tooltip("Checking this causes bob to jump before swinging")]
    [SerializeField] bool m_jumpOnStartSwing;
    [Tooltip("The force the bob jumps at if ^^ has been checked")]
    [SerializeField] Vector3 m_startJumpForce;

    [Space(10)]

    [Tooltip("Increases swing intensity, works well with cases where you want to gain momentum fast")]
    [SerializeField] [Range(0.5f,10)] float m_swingForceMultiplier;

    [Tooltip("The maximum force that can be applied to the simulation in any direction")]
    [SerializeField] float m_maxForce;
    [Tooltip("The minimum force that can be applied to the simulation in any direction")]
    [SerializeField] float m_minForce;

    [Space(5)]
    [Tooltip("If checked, a line is drawn between the bob and the pivot")]
    [SerializeField] bool m_showCord;
    [Tooltip("If ^^ is checked, the width of the line between the bob and the pivot")]
    [SerializeField] [Range(0,1)] float m_cordWidth = 0.1f;
    [Tooltip("if show cord is checked, the colour gradient of the line between the bob and the pivot")]
    [SerializeField] Gradient m_cordColourGradient;


    #endregion

    #region private variables

    float m_ropeLength;
    Vector3 m_startPosition;

    LineRenderer m_lineRenderer;
    Transform m_targetTransform;
    Vector3 m_currentTargetPoint;
    bool m_isSwinging;

    private float m_pendulumForce;

    #endregion

    public override void Awake()
    {
        base.Awake();
        SetVariables();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_isSwinging)
        {
            //if the object is at the end of the "rope"
            if(Vector3.Distance(m_currentTargetPoint, this.transform.position) >= m_ropeLength)
            {
                m_rigidBody.AddForce(CalculateNetForce(), ForceMode.Force);
            }
        }

    }

    void Update()
    {
        if (m_isSwinging)
        {
            DrawDebugLines();
        }
        else
        {
            ClearDebugLines();
        }
    }

    public void DrawDebugLines()
    {
        m_lineRenderer.positionCount = 2;
        Vector3 _startPoint = this.transform.position;
        Vector3 _endPosition = m_currentTargetPoint;

        m_lineRenderer.SetPosition(0, _startPoint);
        m_lineRenderer.SetPosition(1, _endPosition);
    }

    public void ClearDebugLines()
    {
        m_lineRenderer.positionCount = 0;
    }

    public override void SetVariables()
    {
        base.SetVariables();
        m_lineRenderer = this.GetComponent<LineRenderer>();
        m_lineRenderer.startWidth = m_cordWidth;
        m_lineRenderer.colorGradient = m_cordColourGradient;

    }
    public void SetTargetVariables(Vector3 _targetPosition)
    {
        m_startPosition = this.transform.position;
        m_ropeLength = Vector3.Distance(_targetPosition, this.transform.position);
    }

    public void ClearTargetVariables()
    {
        m_startPosition = Vector2.zero;
        m_ropeLength = 0f;
        m_targetTransform = null;
        m_pendulumForce = 0f;
    }

    public void StartSwing(Vector3 _targetPosition)
    {
        m_currentTargetPoint = _targetPosition;
        SetTargetVariables(_targetPosition);
        m_isSwinging = true;

        if(m_jumpOnStartSwing)
        {
            Jump();
        }
       
    }

    public void EndSwing()
    {
        m_isSwinging = false;
        ClearTargetVariables();
    }

    public void Jump()
    {

        Vector3 b = new Vector3(
            CalculateForceDirection().x * m_startJumpForce.x,
                CalculateForceDirection().y * m_startJumpForce.y,
                CalculateForceDirection().z * m_startJumpForce.z);

        m_rigidBody.velocity += b;

    }

    #region math functions
    float CalculateAngle()
    {
        float _angle = Vector3.Angle(this.transform.position - m_currentTargetPoint, Physics.gravity.normalized);
        return Mathf.Deg2Rad * _angle;
    }

    float CalculateCentripetalForce()
    {
        //F = m(v^2) / r

        float _m = m_rigidBody.mass;
        float _v2 = Mathf.Pow(m_rigidBody.velocity.magnitude, 2) * m_swingForceMultiplier;
        float _finalForce = ( _m * _v2 )/ m_ropeLength;
        return _finalForce;
    }

    float CalculateTensionForce()
    {
        // F = mgCos(theta)

        float _mg = m_rigidBody.mass * Physics.gravity.magnitude * m_swingForceMultiplier;
        float _cosTheta = Mathf.Cos(CalculateAngle());
        float _finalForce = _mg * _cosTheta;

        return _finalForce;
    }

    Vector3 CalculateNetForce()
    {


        float _initialForce = CalculateTensionForce() + CalculateCentripetalForce();

        //clamp force

        float _clampedForce = (Mathf.Clamp(_initialForce, m_minForce, m_maxForce));

        m_pendulumForce = _clampedForce;

        //apply direction 

        Vector3 _forceWithDirection = _clampedForce * CalculateForceDirection();

        return _forceWithDirection ;
    }

    Vector3 CalculateForceDirection()
    {
        Vector3 _direction = (m_currentTargetPoint - this.transform.position).normalized;
        return _direction;
    }

    #endregion 

    #region getters and setters

    public Transform TargetTransform { get { return m_targetTransform; } set { m_targetTransform = value; }}

    public Vector3 CurrentTargetPoint { get { return m_currentTargetPoint; } set { m_currentTargetPoint = value; }} 

    public bool IsSwinging { get { return m_isSwinging; } set { m_isSwinging = value;}}

    public Vector3 JumpForce { get { return m_startJumpForce; } set { m_startJumpForce = value; } }

    public Rigidbody RigidBody { get { return m_rigidBody; } set { m_rigidBody = value; }}

    public float SwingSpeed { get { return m_swingForceMultiplier; } set { m_swingForceMultiplier = value; }}

    public float MaxForce { get { return m_maxForce; } set { m_maxForce = value; } }

    public float MinForce { get { return m_minForce; } set { m_minForce = value; } }

    public float PendulumForce { get { return m_pendulumForce;  } }

    public bool JumpOnStartSwing { get { return m_jumpOnStartSwing; } set { m_jumpOnStartSwing = value; } }
    #endregion

}
