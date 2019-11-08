﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    public delegate void DamageEnemy(float attackDamage, float armourPenetration, float magicDamage, float magicResistPenetration, float pureDamage, Turrets turret, Projectile projectile);
    public event DamageEnemy OnTookDamage;

    public delegate void SendEnemy(Transform enemyTransform, float projectileSpeed, int splashRange);
    public event SendEnemy OnSend;

    [Header("Turret Variables")]
    private float attackRange;
    private float attackSpeed; // Measured in attacks per second.
    private float attackDamage;
    private float magicDamage;
    private float pureDamage;
    private float armourPenetration;
    private float magicResistPenetration;
    private float splashRange;
    private float slowAmount;
    private float projectileSpeed;
    private bool turretCanAttack;
    private EnemySpawner enemySpawner;
    private TurretType turretType;
    private AttackType attackType;
    [SerializeField]
    private Projectile projectilePrefab;


    public enum TurretType
    {
        Sorcerer,
        Archer,
        Bomb,
        Ice,
        Crossbow,
        Hellfire,
        Buff,
        Debuff,
        Undefined
    }

    public enum AttackType
    {
        First,
        Last,
        Closest,
        Strongest
    }

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        turretType = TurretType.Archer;
        pureDamage = 5; // TEMP, DELETE LATER
        attackRange = 10;
        attackSpeed = 1;
        attackType = AttackType.Closest;
        projectileSpeed = 50;
        turretCanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyInRange())
        {
            Attack();
        }
    }

    bool EnemyInRange()
    {
        bool inRange = false;
        foreach (Enemy enemy in enemySpawner.enemyList)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < attackRange)
            {
                inRange = true;
            }
        }
        return inRange;
    }

    Transform TargetClosest()
    {
        Transform closestEnemy = null;
        foreach (Enemy enemy in enemySpawner.enemyList)
        {
            if (closestEnemy == null)
            {
                closestEnemy = enemy.transform;
            }

            else
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(closestEnemy.transform.position, transform.position))
                {
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }

    Transform TargetFirst()
    {
        return null;
    }

    Transform TargetStrongest()
    {
        return null;
    }

    Transform TargetLast()
    {
        return null;
    }

    void Attack()
    {
        if (turretCanAttack)
        {
            Transform targetedEnemy;
            turretCanAttack = false;
            switch (attackType)
            {
                case AttackType.First:
                    targetedEnemy = TargetFirst();
                    break;

                case AttackType.Closest:
                    targetedEnemy = TargetClosest();
                    break;

                case AttackType.Strongest:
                    targetedEnemy = TargetStrongest();
                    break;

                case AttackType.Last:
                    targetedEnemy = TargetLast();
                    break;

                default:
                    targetedEnemy = null;
                    Debug.Log("Cannot Attack");
                    return;
            }
            
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
            if (OnSend != null & targetedEnemy != null)
            {
                OnSend(targetedEnemy, projectileSpeed, Mathf.RoundToInt(splashRange));
            }
            else
            {
                Debug.Log("No Target");
            }
            projectile.OnEnemyHit += Projectile_OnDamageDealt;

            StartCoroutine(AttackCooldown());
        }
    }

    private void Projectile_OnDamageDealt(Projectile projectile)
    {
        if (OnTookDamage != null)
        { 
            OnTookDamage(attackDamage, armourPenetration, magicDamage, magicResistPenetration, pureDamage, this, projectile);
        }
        else
        {
            Debug.Log("Enemy did not take damage");
        }
        projectile.OnEnemyHit -= Projectile_OnDamageDealt;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeed);
        turretCanAttack = true;
    }
}
