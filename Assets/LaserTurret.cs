using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour {
    [SerializeField]
    private AudioSource sound;

    private const float shootSpeed = 1f;

    private float lastFire = 0;

    public void FireLaser(float angle, float speed)
    {
        if (Time.realtimeSinceStartup - lastFire > shootSpeed * Time.deltaTime)
        {
            sound.Play();
            lastFire = Time.realtimeSinceStartup;
        }
    }

    private Sprite GenerateSprite(string name, Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture);
    }

}
