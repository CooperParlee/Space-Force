using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour {
    [SerializeField]
    private AudioSource sound;
    private Tracking shipMain;

    private const float shootSpeed = 1f;

    private float lastFire = 0;

    public void Init (Tracking shipMain)
    {
        this.shipMain = shipMain;
    }

    public void FireLaser(float angle, float speed)
    {
        if (Time.realtimeSinceStartup - lastFire > shootSpeed * Time.deltaTime)
        {
            sound.Play();

            GameObject pulse = new GameObject();
            pulse.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            SpriteRenderer renderer = pulse.AddComponent<SpriteRenderer>() as SpriteRenderer;
            renderer.sprite = GenerateSprite(Resources.Load<Texture2D>("Assets/Resources/Sprites/Laser_Turret"));
            pulse.transform.position = shipMain.transform.position;

            lastFire = Time.realtimeSinceStartup;
        }
    }

    private Sprite GenerateSprite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(texture.width/2, texture.height/2), 5);

        return sprite;
    }

}
