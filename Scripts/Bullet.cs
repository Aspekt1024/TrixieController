using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeSpawned;
    private float maxLifetime = 2f;

    public void Fire(Vector2 origin, Vector2 direction, float speed)
    {
        transform.position = origin;
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
        timeSpawned = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;

        var damageableObject = collision.GetComponent<IDamageable>();
        damageableObject?.TakeDamage(1);
        Explode();
    }
    
    private void Update()
    {
        if (Time.time > timeSpawned + maxLifetime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
