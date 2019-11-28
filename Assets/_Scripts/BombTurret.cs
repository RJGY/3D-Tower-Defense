using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTurret : Turrets
{
    // BASE STATS
    protected new float worth = 80;
    protected new float attackDamage = 5;
    protected new float magicDamage = 2;
    protected new float pureDamage = 0;
    protected new float armourPenetration = 70;
    protected new float magicResistPenetration = 20;
    protected new float splashRange = 3;
    protected new float attackRange = 9;
    protected new float attackSpeed = 0.8f;
    protected new float projectileSpeed = 20;

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
        return worth;
    }

}
