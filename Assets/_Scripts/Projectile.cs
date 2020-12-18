using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    private LayerMask enemyLayer;
    private Transform enemy;
    private Rigidbody rb;
    private float projectileSpeed;
    [SerializeField] private float multiplier;

    public void AssignStats(float physicalDamage, float magicDamage, LayerMask enemyLayer, Transform enemy, float speed)
    {
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
        this.enemyLayer = enemyLayer;
        this.enemy = enemy;
        this.projectileSpeed = speed;
        rb = FindObjectOfType<Rigidbody>();
    }


    private void Update()
    {
        if (enemy != null)
        {
            // raycast
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.forward);

            Debug.DrawRay(transform.position, transform.forward, Color.red);

            if (Physics.Raycast(ray, out hit, 0.2f, enemyLayer))
            {
                DealDamage(enemy.GetComponent<Entity>());
                Destroy(gameObject);
            }

            transform.LookAt(enemy);

            rb.velocity = transform.forward * projectileSpeed * Time.deltaTime;


        }
        else
        {
            Debug.Log("no target");
            Destroy(gameObject);
        }
    }
}
