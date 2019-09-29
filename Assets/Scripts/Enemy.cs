using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 2f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 0.2f;
    [Header("Death")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float deathDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void Fire()
    {
        var projectileSpawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        var proj = Instantiate(projectile, projectileSpawnPoint, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1 * projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (damageDealer != null)
        {
            health -= damageDealer.GetDamage();
        }
        if (health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
        var explosion = Instantiate(
            deathVFX,
            transform.position,
            transform.rotation
        );
        Destroy(explosion, deathDuration);
    }
}
