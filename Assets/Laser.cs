using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    float angle;
    float speed;
    float range; 

    Vector3 origin;
    Vector3 target;
    private const float xOffset = -0.6f;
    private const float yOffset = -2f;

    public void Init(float angle, Vector3 target, float speed = 40f, float range = 100f)
    {
        this.origin = gameObject.transform.position + new Vector3(yOffset * Mathf.Sin(angle * Mathf.Deg2Rad) + xOffset * Mathf.Cos(angle * Mathf.Deg2Rad), -yOffset * Mathf.Cos(angle * Mathf.Deg2Rad) + xOffset * Mathf.Sin(angle * Mathf.Deg2Rad)); //Right turret new Vector3(yOffset * Mathf.Sin(angle * Mathf.Deg2Rad), -yOffset * Mathf.Cos(angle * Mathf.Deg2Rad));
        this.angle = Mathf.Atan((target.y - origin.y) / (target.x - origin.x)) * Mathf.Rad2Deg - angle;
        this.speed = speed;
        this.range = range;
        this.target = target;
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, angle + 90f);
        gameObject.transform.position = origin;
        gameObject.name = "Laser Beam";
    }

    // Update is called once per frame
	void Update () {
        gameObject.transform.Translate(new Vector3(speed * Mathf.Sin(angle * Mathf.Deg2Rad) * Time.deltaTime, speed * Mathf.Cos(angle * Mathf.Deg2Rad) * Time.deltaTime), Space.World);
        if((gameObject.transform.position - origin).magnitude >= range)
        {
            Destroy(gameObject);
        }
	}
}
