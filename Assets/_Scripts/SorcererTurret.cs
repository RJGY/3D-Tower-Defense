using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererTurret : Turrets
{
    public new float Worth = 80;

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
        attackDamage = 0;
        magicDamage = 15;
        pureDamage = 0;
        armourPenetration = 50;
        magicResistPenetration = 100;
        splashRange = 2;
        attackRange = 10;
        attackSpeed = 1.2f;
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
        return TurretType.Sorcerer;
    }

    public override float GetTurretCost()
    {
        return Worth;
    }

    public override void UpgradeTurret()
    {
        base.UpgradeTurret();
        worth *= 1.2f;
    }
}
