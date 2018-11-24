using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour {
    public new Camera camera;
    public Animator animator;
    public Controls controls;

    private const float RotP = 0.105f; // The proportional constant for the rotation of the character
    private const float AccPX = 0.001f; // The proportional constant for the forward acceleration of the character
    private const float ACCPY = 0.001f; // The proportional constants for the horizontal acceleration of the character
    private const float DecP = 0.001f; // The proportional constant for the inertial dampeners of the ship
    private const float SMax = 0.05f; // The max speed in any given direction

    private float m_XSpeed = 0.0f;
    private float m_YSpeed = 0.0f;
    private float m_CurrentRot;
    private float m_LastPowerX = 0.0f;
    private float m_LastPowerY = 0.0f;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFeedForwardController();
        UpdateFeedForwardAcceleration();

    }

    public float GetRadToMouse()
    {
        Vector3 position = gameObject.transform.position;
        Vector3 mousepos = this.camera.ScreenToWorldPoint(Input.mousePosition);

        float xdif = mousepos.x - position.x;
        float ydif = mousepos.y - position.y;

        float rad = Mathf.Atan2(ydif, xdif);

        return rad;
    }

    private void UpdateFeedForwardController()
    {
        m_CurrentRot = gameObject.transform.rotation.eulerAngles.z - 90;
        float target = GetRadToMouse() * Mathf.Rad2Deg + 180;
        float error = target - m_CurrentRot;
        
        if (error > 180)
        {
            error = error - 360;
        }
        if (error < -180)
        {
            error = error + 360;
        }
        
        float power = error * RotP;
        gameObject.transform.eulerAngles = new Vector3(0, 0, m_CurrentRot + power + 90); 
        
    }

    private void UpdateFeedForwardAcceleration()
    {
        if(controls.GetForward() != 0) // Animation stuff
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        float modRads = (m_CurrentRot + 90) * Mathf.Deg2Rad;
        float forwardDesired = controls.GetForward(); // What the player wants the power to be/maximum.
        float horDesired = controls.GetHorizontal();

        float xApp = -Mathf.Sin(modRads); // What percent of power will be applied to each axis.
        float yApp = Mathf.Cos(modRads);

        //float forwardError = forwardDesired - m_LastPowerX

        print("xTrig " + xApp);
        print("yTrig " + yApp);
        print("Rad " + modRads);
        //gameObject.transform.Translate(new Vector3(xSpeed, ySpeed, 0));
    }

}
