using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour {
    public new Camera camera;
    public Animator animator;
    public Controls controls;
    public AudioSource sound;

    private const float RotP = 0.105f; // The proportional constant for the rotation of the character
    private const float AccP = 0.03f; // The proportional constant for the forward acceleration of the character
    private const float DecP = 0.001f; // The proportional constant for the inertial dampeners of the ship
    private const float SMax = 0.07f; // The max speed in any given direction
    private const float SMaxStrafe = 0.04f;
    private const float SMaxReverseTarget = -0.40f;

    private float xPower = 0.0f;
    private float yPower = 0.0f;
    private float currentRot;
    private float lastPowerX = 0.0f;
    private float lastPowerY = 0.0f;
    
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
        currentRot = gameObject.transform.rotation.eulerAngles.z - 90;
        float target = GetRadToMouse() * Mathf.Rad2Deg + 180;
        float error = target - currentRot;
        
        if (error > 180)
        {
            error = error - 360;
        }
        if (error < -180)
        {
            error = error + 360;
        }
        
        float power = error * RotP;
        gameObject.transform.eulerAngles = new Vector3(0, 0, currentRot + power + 90); 
        
    }

    private void UpdateFeedForwardAcceleration()
    {
        if(controls.GetForward() != 0) // Animation stuff
        {
            animator.SetBool("IsMoving", true);
            if (!sound.isPlaying)
            {
                sound.Play();
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
            sound.Stop();
        }
        float modRads = (currentRot + 90) * Mathf.Deg2Rad;
        float forwardDesired = controls.GetForward(); // What the player wants the power to be/maximum.
        float horDesired = -controls.GetHorizontal();

        float xApp = Mathf.Sin(-modRads); // What percent of power will be applied to each axis.
        float yApp = Mathf.Cos(modRads);

        float xAppHorizontal = -Mathf.Sin(modRads + 90 * Mathf.Deg2Rad);
        float yAppHorizontal = Mathf.Cos(modRads + 90 * Mathf.Deg2Rad);

        forwardDesired = Mathf.Max(SMaxReverseTarget, forwardDesired);

        float xTarget = xApp * SMax * forwardDesired + xAppHorizontal * SMaxStrafe * horDesired; //PID stuff
        float yTarget = yApp * SMax * forwardDesired + yAppHorizontal * SMaxStrafe * horDesired;
        float xError = xTarget - lastPowerX;
        float yError = yTarget - lastPowerY;
        xPower = xError * AccP;
        yPower = yError * AccP;

        print("Target " + xTarget);
        print("Last " + lastPowerX);
        //gameObject.transform.TransformVector(gameObject.transform.position + new Vector3(xSpeed, ySpeed, 0));
        gameObject.transform.Translate(new Vector3(lastPowerX + xPower, lastPowerY + yPower, 0), Space.World);
        lastPowerX = xPower + lastPowerX;
        lastPowerY = yPower + lastPowerY;
    }

}
