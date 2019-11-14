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
    private float sellValue;
    private TurretHead turretHead;
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
        turretHead = GetComponentInChildren<TurretHead>();
    }

    // Start is called before the first frame update
    void Start()
    {
        turretType = TurretType.Archer;
        pureDamage = 2.5f; // TEMP, DELETE LATER
        attackRange = 10;
        attackSpeed = 1;
        attackType = AttackType.Last;
        splashRange = 10;
        projectileSpeed = 30;
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
                break;
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
                if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(closestEnemy.position, transform.position))
                {
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }

    Transform TargetFirst()
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

    Transform TargetStrongest()
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

    Transform TargetLast()
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

    void Attack()
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

    private void Projectile_OnSplashDamageDealt(Projectile projectile)
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

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeed);
        turretCanAttack = true;
    }
}
