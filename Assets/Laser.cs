using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    float rawAngleToTarget;
    float speed;
    float range;
    float newAngle;

    Vector3 origin;
    Vector3 target;
    private const int rangeOfMotion = 15;

    public void Init(float shipAngle, Vector3 target, float speed = 40f, float range = 100f)
    {
        this.speed = speed;
        this.range = range;
        this.target = target;
        shipAngle = shipAngle + 90f;

        this.origin = gameObject.transform.position;
        this.rawAngleToTarget = Mathf.Atan2(target.y - origin.y, target.x - origin.x) * Mathf.Rad2Deg;
        float error = rawAngleToTarget - shipAngle;
        //this.rawAngleToTarget = Mathf.Max(Mathf.Min(this.rawAngleToTarget, rangeOfMotion), -rangeOfMotion);
        this.newAngle = shipAngle + error;

        gameObject.transform.eulerAngles = new Vector3(0f, 0f, newAngle);
        gameObject.transform.position = origin;
        gameObject.name = "Laser Beam";

        print("Ship angle " + shipAngle);
        print("Desired angle " + rawAngleToTarget);
        print("Error " + error);
        print("Pulse angle " + newAngle);
    }

    // Update is called once per frame
	void Update () {
        gameObject.transform.Translate(new Vector3(speed * Mathf.Cos(newAngle * Mathf.Deg2Rad) * Time.deltaTime, speed * Mathf.Sin(newAngle * Mathf.Deg2Rad) * Time.deltaTime), Space.World);
        if((gameObject.transform.position - origin).magnitude >= range)
        {
            Destroy(gameObject);
        }
	}
}
