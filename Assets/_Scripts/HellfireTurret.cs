using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellfireTurret : Turrets
{
    public new float Worth = 500;

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
        attackDamage = 15;
        magicDamage = 15;
        pureDamage = 15;
        armourPenetration = 50;
        magicResistPenetration = 100;
        splashRange = 15;
        attackRange = 10;
        attackSpeed = 1f;
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
        return TurretType.Hellfire;
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
