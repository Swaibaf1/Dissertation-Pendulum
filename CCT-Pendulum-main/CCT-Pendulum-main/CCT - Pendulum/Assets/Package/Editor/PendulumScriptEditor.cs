using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;
using System.Drawing.Printing;

[CustomEditor(typeof(PendulumScript))]
[ExecuteInEditMode]
[CanEditMultipleObjects]

[System.Serializable]
public class PendulumScriptEditor : Editor
{
    PendulumScript m_script;
  
    private void OnEnable ()
    {
        m_script = (PendulumScript)target; 
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        if (GUILayout.Button("Default Values"))
        {
            //values
            m_script.Mass = 1f;
            m_script.CustomTimeStepValue = 0.001f;
            m_script.MaxForce = 100f;
            m_script.MinForce = 5f;
            m_script.Drag = 0.001f;

            m_script.JumpForce = new Vector3(4,6,4);
            m_script.SwingSpeed = 1.2f;

            //booleans
            m_script.Interpolate = true;
            m_script.UseCustomTimeStep = false;
            m_script.IsSwinging = false;
        }

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("NET FORCE = Mass * Gravity * Cos(Angle) + Mass * Velocity^2 / Rope Length");

        EditorGUILayout.LabelField("Current Net Force: " + m_script.PendulumForce.ToString());


        base.OnInspectorGUI();
        this.serializedObject.ApplyModifiedProperties();
    }
}
