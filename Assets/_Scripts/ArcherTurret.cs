using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTurret : Turrets
{
    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        turretHead = GetComponentInChildren<TurretHead>();
    }

    // Start is called before the first frame update
    void Start() 
    {
        // BASE STATS
        // TODO LOAD PREFAB OF RESEOURCES ARROW
        
        attackDamage = 5;
        magicDamage = 0;
        pureDamage = 0;
        armourPenetration = 50;
        magicResistPenetration = 0;
        splashRange = -1;
        attackRange = 10;
        attackSpeed = 2;
        projectileSpeed = 40;
        attackType = AttackType.First;

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

    public override TurretType GetTurretType()
    {
        return TurretType.Archer;
    }
}
