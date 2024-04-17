using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public static ItemPickup instance;

    public bool isHealth = false;
    public bool isAmmo = false;

    public float speed = 10;

    public AudioClip pickupSound;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Player")
        {
            if (isHealth)
            {
                PlayerController.main.Health += 1;
            }
            else if (isAmmo)
            {
                Weapon.instance.currAltAmmo += 1;
            }

            AudioSource.PlayClipAtPoint(pickupSound, new Vector2(9, 0), 1f);
            Destroy(gameObject);
        }
    }
}
