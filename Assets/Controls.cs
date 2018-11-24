using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    readonly float horizontalSpeed = 0.25f;
    readonly float verticalSpeed = 0.75f;

    private bool m_Fire = false;
    private float m_Strafe = 0.0f;
    private float m_Forward = 0.0f;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            m_Fire = true;
        }
        m_Forward = Input.GetAxisRaw("Vertical");
        m_Strafe = Input.GetAxisRaw("Horizontal");
        
	}

    public bool AwaitFireInterrupt()
    {
        if(m_Fire == true)
        {
            m_Fire = false;
            return true;
        }
        return false;
    }
    public float GetForward()
    {
        return m_Forward;
    }
    public float GetHorizontal()
    {
        return m_Strafe;
    }
}
