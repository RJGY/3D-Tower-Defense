using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("")]

    [Header("Combat Variables")]
    protected float attackRange;
    protected float physicalDamage;
    protected float magicDamage;
    protected float armourPenetration;
    protected float magicResistPenetration;
    protected float attackSpeed;
    protected bool canAttack;
    protected bool isAttackReseting;
    protected float maxHealth;
    protected float health;
    protected float armour;
    protected float magicResist;
    protected bool isMelee;
    protected bool stoppedToAttack;
    #region Functions

    protected void AssignStats()
    {
        health = maxHealth;
        canAttack = true;
    }

    protected void DealDamage(Entity enemy)
    {
        enemy.TakeDamage(physicalDamage, magicDamage);
    }
    
    public void TakeDamage(float physDamage, float magicDamage)
    {
        health -= (physDamage/((100 + armour)/100)) + (magicDamage/((100 + magicResist)/100));

        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        // Destroy gameobject.
        Destroy(gameObject);
    }

    #endregion

    #region Coroutines

    #endregion
}
