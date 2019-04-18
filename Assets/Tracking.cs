using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour {
    public new Camera camera;
    public Animator animator;
    public Controls controls;
    public AudioSource sound;
    private LaserTurret laser;

    private const float RotP = 0.105f; // The proportional constant for the rotation of the character
    private const float AccP = 0.03f; // The proportional constant for the forward acceleration of the character
    private const float DecP = 0.001f; // The proportional constant for the inertial dampeners of the ship
    private const float CameraP = 0.03f;

    private const float SMax = 0.10f; // The max speed in any given direction
    private const float SMaxStrafe = 0.02f;
    private const float SMaxReverseTarget = -0.20f;

    private const float ControllerTurnSensitivity = 300f;

    private const float LargeStarTransFactor = 1f;
    private const float SmallStarTransFactor = 0.5f;

    private float xPower = 0.0f;
    private float yPower = 0.0f;
    private float currentRot;
    private float lastPowerX = 0.0f;
    private float lastPowerY = 0.0f;
    private float target = 0;

    private bool isController = false;
    
    // Use this for initialization
    void Start () {
        laser = gameObject.AddComponent<LaserTurret>();
        laser.Init(this);
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePIDOrientation();
        UpdatePIDAcceleration();
        UpdateLaserTurret();
        TrackCameraToShip();

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

    private void UpdatePIDOrientation()
    {
        currentRot = gameObject.transform.rotation.eulerAngles.z - 90;
        if (!isController)
        {
            target = GetRadToMouse() * Mathf.Rad2Deg + 180;
        }
        else if (isController)
        {
            target = target + -controls.GetRightStickX() * Time.deltaTime * ControllerTurnSensitivity;
            print(target);
            if (target > 360)
            {
                target = target - 360;
            }
            if (target < -360)
            {
                target = target + 360;
            }
        }
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
    private void UpdateLaserTurret()
    {
        if (controls.AwaitFireInterrupt())
        {
            laser.FireLaser(90, 1);
        }
    }
    private void UpdatePIDAcceleration()
    {
        if(controls.GetLeftStickY() > 0) // Animation stuff
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
        float forwardDesired = controls.GetLeftStickY(); // What the player wants the power to be/maximum.
        float horDesired = -controls.GetLeftStickX();

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

        //gameObject.transform.TransformVector(gameObject.transform.position + new Vector3(xSpeed, ySpeed, 0));
        gameObject.transform.Translate(new Vector3(lastPowerX + xPower, lastPowerY + yPower, 0), Space.World);
        lastPowerX = xPower + lastPowerX;
        lastPowerY = yPower + lastPowerY;
    }

    private void TrackCameraToShip()
    {
        float errorX = gameObject.transform.position.x - camera.transform.position.x;
        float errorY = gameObject.transform.position.y - camera.transform.position.y;
        float cameraPowerX = errorX * CameraP;
        float cameraPowerY = errorY * CameraP;

        camera.transform.Translate(new Vector3(cameraPowerX, cameraPowerY, 0));
    }

    public Vector3 GetShipPosition()
    {
        return gameObject.transform.position;
    }

}
