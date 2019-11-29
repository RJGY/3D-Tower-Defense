using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    public delegate void DamageEnemy(float attackDamage, float armourPenetration, float magicDamage, float magicResistPenetration, float pureDamage, Turrets turret, Projectile projectile);
    public event DamageEnemy OnTookDamage;

    public delegate void SendEnemy(Transform enemyTransform, float projectileSpeed, float splashRange);
    public event SendEnemy OnSend;

    public delegate void LookAtEnemy(Vector3 enemyDirection);
    public event LookAtEnemy OnLookAtEnemy;

    [Header("Turret Variables")]
    protected float attackRange;
    protected float attackSpeed; // Measured in attacks per second.
    protected float attackDamage;
    protected float magicDamage;
    protected float pureDamage;
    protected float armourPenetration;
    protected float magicResistPenetration;
    protected float splashRange;
    protected float slowAmount;
    protected float projectileSpeed;
    protected bool turretCanAttack;
    protected TurretHead turretHead;
    protected EnemySpawner enemySpawner;
    protected TurretType turretType;
    protected AttackType attackType;
    protected float worth;
    protected bool buffed;
    protected float currentBuffValue;
    [SerializeField]
    protected Projectile projectilePrefab;


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
        Dark,
        Undefined
    }

    public enum AttackType
    {
        First,
        Last,
        Closest,
        Strongest
    }

    virtual protected bool EnemyInRange()
    {
        bool inRange = false;
        foreach (Enemy enemy in enemySpawner.enemyList)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < attackRange)
            {
                inRange = true;
                break;
            }
        }
        return inRange;
    }

    protected Transform TargetClosest()
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
                if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(closestEnemy.position, transform.position))
                {
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }

    protected Transform TargetFirst()
    {
        Transform firstEnemy = null;
        foreach (Enemy enemy in enemySpawner.enemyList)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) > attackRange)
            {
                continue;
            }

            firstEnemy = enemy.transform;
            break;
        }
        return firstEnemy;
    }

    protected Transform TargetStrongest()
    {
        Enemy strongestEnemy = null;
        foreach (Enemy enemy in enemySpawner.enemyList)
        {
            // Enemy out of attack range
            if (Vector3.Distance(enemy.transform.position, transform.position) > attackRange)
            {
                // Dont bother with targeting it.
                continue;
            }

            if (strongestEnemy == null)
            {
                strongestEnemy = enemy;
            }
            else
            {
                // Make this an event.
                if (strongestEnemy.GetHealth() < enemy.GetHealth())
                {
                    strongestEnemy = enemy;
                }
            }
        }

        return strongestEnemy.transform;
    }

    protected Transform TargetLast()
    {
        Transform lastEnemy = null;
        foreach (Enemy enemy in enemySpawner.enemyList)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) > attackRange)
            {
                continue;
            }

            lastEnemy = enemy.transform;
        }
        return lastEnemy;
    }

    virtual protected void Attack()
    {
        Transform targetedEnemy;
        Vector3 targetPosition;
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

        // MAKE THIS AN EVENT.
        targetPosition = targetedEnemy.position - transform.position;
        if (OnLookAtEnemy != null)
        {
            OnLookAtEnemy(targetPosition);
        }
        else
        {
            Debug.Log("Turret head isn't connected.");
        }

        if (turretCanAttack)
        {
            turretCanAttack = false;

            Projectile projectile = Instantiate(projectilePrefab, turretHead.transform.position, Quaternion.identity, transform);
            if (OnSend != null & targetedEnemy != null)
            {
                OnSend(targetedEnemy, projectileSpeed, splashRange);
            }
            else
            {
                Debug.Log("No Target");
            }
            projectile.OnEnemyHit += Projectile_OnDamageDealt;
            projectile.OnSplashHit += Projectile_OnSplashDamageDealt;
            StartCoroutine(AttackCooldown());
        }
    }

    protected void Projectile_OnDamageDealt(Projectile projectile)
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

    protected void Projectile_OnSplashDamageDealt(Projectile projectile)
    {
        if (OnTookDamage != null)
        {
            OnTookDamage(attackDamage * 0.5f, armourPenetration, magicDamage * 0.5f, magicResistPenetration, pureDamage, this, projectile);
        }
        else
        {
            Debug.Log("Enemy did not take splash damage");
        }

        projectile.OnEnemyHit -= Projectile_OnSplashDamageDealt;
    }

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeed);
        turretCanAttack = true;
    }

    public virtual TurretType GetTurretType()
    {
        return turretType;
    }

    public virtual float GetTurretCost()
    {
        return worth;
    }

    public virtual void BuffTurret(float multiplier)
    {
        if (!buffed)
        {
            attackDamage *= multiplier;
            magicDamage *= multiplier;
            pureDamage *= multiplier;
            attackRange *= multiplier;
            attackSpeed *= multiplier;

            currentBuffValue = multiplier;
            buffed = true;
            StartCoroutine(BuffTimer());
        }
    }

    IEnumerator BuffTimer()
    {
        yield return new WaitForSeconds(10f);
        yield return new WaitForEndOfFrame();
        attackDamage /= currentBuffValue;
        magicDamage /= currentBuffValue;
        pureDamage /= currentBuffValue;
        attackRange /= currentBuffValue;
        attackSpeed /= currentBuffValue;
        buffed = false;
    }

    public virtual void RemoveBuffs()
    {
        if (!buffed)
        {
            return;
        }

        StopCoroutine(BuffTimer());
        attackDamage /= currentBuffValue;
        magicDamage /= currentBuffValue;
        pureDamage /= currentBuffValue;
        attackRange /= currentBuffValue;
        attackSpeed /= currentBuffValue;
        buffed = false;
    }

    public virtual void UpgradeTurret()
    {
        attackDamage *= 1.2f;
        magicDamage *= 1.2f;
        pureDamage *= 1.2f;
        attackRange *= 1.2f;
        attackSpeed *= 1.2f;
        worth *= 1.2f;
    }
}
