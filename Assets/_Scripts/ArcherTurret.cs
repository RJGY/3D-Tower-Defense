using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTurret : Turrets
{
    public new float worth { get; protected set; } = 50;

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

        // BASE STATS
        attackDamage = 8;
        magicDamage = 0;
        pureDamage = 0;
        armourPenetration = 50;
        magicResistPenetration = 0;
        splashRange = -1;
        attackRange = 12;
        attackSpeed = 2;
        projectileSpeed = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyInRange())
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override bool EnemyInRange()
    {
        return base.EnemyInRange();
    }

    public override TurretType GetTurretType()
    {
        return TurretType.Archer;
    }

    public override float GetTurretCost()
    {
        return worth;
    }

}
