using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTurret : Turrets
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
        // TODO LOAD PREFAB OF RESEOURCES BOMB
        turretType = TurretType.Archer;
        attackDamage = 1;
        magicDamage = 0;
        pureDamage = 0;
        armourPenetration = 50;
        magicResistPenetration = 0;
        splashRange = 10;
        attackRange = 10;
        attackSpeed = 1;
        projectileSpeed = 30;
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
}
