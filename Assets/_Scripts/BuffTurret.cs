using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTurret : Turrets
{
    // THIS DUDE NEEDS NEW ATTACK FUNCTION
    // THis dude doesnt need a projectile
    // BASE STATS
    protected new float worth = 500;
    protected new float attackDamage = 1.5f;
    protected new float magicDamage = 0;
    protected new float pureDamage = 0;
    protected new float armourPenetration = 0;
    protected new float magicResistPenetration = 0;
    protected new float splashRange = -1;
    protected new float attackRange = 15;
    protected new float attackSpeed = 1;
    protected new float projectileSpeed = 1000;
    protected new Projectile projectilePrefab = null;
    private List<Turrets> turrets;
    [SerializeField]
    private NodeHolder nodeHolder;
    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        turretHead = GetComponentInChildren<TurretHead>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO LOAD PREFAB OF RESEOURCES ARROW
        attackType = AttackType.First;
        turretCanAttack = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TowerInRange())
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        foreach (Turrets turret in turrets)
        {
            if (Vector3.Distance(turret.transform.position, transform.position) < attackRange)
            {
                // Buff the turret
                turret.BuffTurret(attackDamage);
            }
        }
    }

    protected bool TowerInRange()
    {
        GetTurretsInList();
        if (turrets.Count > 0)
        {
            foreach (Turrets turret in turrets)
            {
                if (Vector3.Distance(turret.transform.position, transform.position) < attackRange)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void GetTurretsInList()
    {
        foreach (Node node in nodeHolder.GetComponentsInChildren<Node>())
        {
            if (node.GetComponentInChildren<Turrets>() != null && node.GetComponentInChildren<BuffTurret>() == null)
            {
                turrets.Add(node.GetComponentInChildren<Turrets>());
            }
        }
    }

    public override TurretType GetTurretType()
    {
        return TurretType.Buff;
    }

    public override float GetTurretCost()
    {
        return worth;
    }
}
