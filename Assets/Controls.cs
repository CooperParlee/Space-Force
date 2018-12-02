using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    private bool m_Fire = false;
    private float m_Strafe = 0.0f;
    private float m_Forward = 0.0f;
    private float m_Turn = 0.0f;
    
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
        m_Turn = Input.GetAxisRaw("JoyRightX");
        print("Yjoy" + m_Turn);
        print("XHor" + m_Strafe);
        print("XVert" + m_Forward);
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
    public float GetLeftStickY()
    {
        return m_Forward;
    }
    public float GetLeftStickX()
    {
        return m_Strafe;
    }
    public float GetRightStickX()
    {
        return m_Turn;
    }
}
