using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy stats")]
    public float Health = 3.0f;
    public float moveSpeed = 4.0f;
    public float shootCooldown;
    public int Points = 300;

    [Header("Enemy objects")]
    public GameObject deathEffect;
    public GameObject Projectile;

    public GameObject[] itemDrops;

    public Transform firePoint;

    public AudioClip gunSound;
    public AudioClip deathSound;

    float canShoot;

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (canShoot < Time.time)
        {
            Shoot();
            canShoot = Time.time + shootCooldown;
        }
    }

    private void Shoot()
    {
        AudioSource.PlayClipAtPoint(gunSound, new Vector2(9, 0), 1f);
        Instantiate(Projectile, firePoint.position, firePoint.rotation);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
            
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSound, new Vector2(9, 0), 1f);
        GameController.instance.combo++;
        GameController.instance.updateScore(Points * GameController.instance.combo);
        dropItem();
    }

    private void dropItem()
    {
        if (Random.value > 0.8)
        {
            if (Random.value > 0.5)
            {
                Instantiate(itemDrops[0], firePoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(itemDrops[1], firePoint.position, Quaternion.identity);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Player")
        {
            PlayerController player = hitInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Die();
        }
    }
}
