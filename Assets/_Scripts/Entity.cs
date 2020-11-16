using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Combat Variables")]
    [SerializeField] private float attackRange;
    private float physicalDamage;
    private float magicDamage;
    private float attackRate;
    private bool canAttack;
    private bool isAttackReseting;
    private float maxHealth;
    private float health;
    private float armour;
    private float magicResist;

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

    private void Attack()
    {
        
    }

    #endregion

    #region Coroutines
    protected IEnumerator AttackCooldown()
    {
        if (!isAttackReseting && !canAttack)
        {
            isAttackReseting = true;
            yield return new WaitForSeconds(attackRate);
            canAttack = true;
            isAttackReseting = false;
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
