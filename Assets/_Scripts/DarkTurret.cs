using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTurret : Turrets
{
    public new float Worth = 100;

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
        attackDamage = 5;
        magicDamage = 5;
        pureDamage = 1;
        armourPenetration = 50;
        magicResistPenetration = 50;
        splashRange = 2;
        attackRange = 10;
        attackSpeed = 1.5f;
        projectileSpeed = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyInRange())
        {
            Attack();
        }
    }

    public override TurretType GetTurretType()
    {
        return TurretType.Dark;
    }

    public override float GetTurretCost()
    {
        return Worth;
    }
}
