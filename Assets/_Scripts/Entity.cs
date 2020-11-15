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
    private float maxHealth;
    private float health;
    private float armour;
    private float magicResist;
    private Transform targetedTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForEndOfFrame();
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
        //Destroy gameobject
        Destroy(gameObject);
    }
}
