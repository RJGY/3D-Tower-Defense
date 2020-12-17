using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    public LayerMask enemyLayer;
    public void AssignStats(float physicalDamage, float magicDamage, LayerMask enemyLayer)
    {
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
        this.enemyLayer = enemyLayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            DealDamage(other.GetComponent<Entity>());
        }
    }
}
