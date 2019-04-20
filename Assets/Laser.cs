using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    float speed;
    float range;
    float angle;

    Vector3 origin;
    Vector3 target;
    private const int rangeOfMotion = 15;

    public void Fire(float shipAngle, Vector3 origin, Vector3 target, float speed = 40f, float range = 100f)
    {
        this.speed = speed;
        this.range = range;
        this.target = target;
        shipAngle = shipAngle + 90f;

        this.origin = origin;
        float rawAngleToTarget = Mathf.Atan2(target.y - origin.y, target.x - origin.x) * Mathf.Rad2Deg;
        float error = rawAngleToTarget - shipAngle;
        //this.rawAngleToTarget = Mathf.Max(Mathf.Min(this.rawAngleToTarget, rangeOfMotion), -rangeOfMotion);
        this.angle = shipAngle + error;

        gameObject.transform.eulerAngles = new Vector3(0f, 0f, angle);
        gameObject.transform.position = origin;
        gameObject.name = "Laser Beam";

        print("Ship angle " + shipAngle);
        print("Desired angle " + rawAngleToTarget);
        print("Error " + error);
        print("Pulse angle " + angle);
    }

    // Update is called once per frame
	void Update () {
        gameObject.transform.Translate(new Vector3(speed * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.deltaTime, speed * Mathf.Sin(angle * Mathf.Deg2Rad) * Time.deltaTime), Space.World);
        if((gameObject.transform.position - origin).magnitude >= range)
        {
            Destroy(gameObject);
        }
	}
}
