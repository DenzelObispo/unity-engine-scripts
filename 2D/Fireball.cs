using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float moveSpeed = 5f;
    public AudioClip fire;
    float damage;

    void Start()
    {
        AudioSource.PlayClipAtPoint(fire, new Vector2(9, 0), .5f);
    }


    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < -3)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Player")
        {
            PlayerController player = hitInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                damage = player.Health;
                player.TakeDamage(damage);
            }
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
