using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTurret : Turrets
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
        attackDamage = 12;
        magicDamage = 0;
        pureDamage = 0;
        armourPenetration = 100;
        magicResistPenetration = 20;
        splashRange = 5;
        attackRange = 10;
        attackSpeed = 0.8f;
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
        return TurretType.Bomb;
    }

    public override float GetTurretCost()
    {
        return Worth;
    }

    public override void UpgradeTurret()
    {
        base.UpgradeTurret();
        Worth *= 1.2f;
    }

}
