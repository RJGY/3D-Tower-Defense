using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTurret : Turrets
{
    // THIS DUDE NEEDS NEW ATTACK FUNCTION
    // BASE STATS
    protected new float worth = 50;
    protected new float attackDamage = 0;
    protected new float magicDamage = 0;
    protected new float pureDamage = 0;
    protected new float armourPenetration = 100;
    protected new float magicResistPenetration = 0;
    protected new float splashRange = 3;
    protected new float attackRange = 15;
    protected new float attackSpeed = 1;
    protected new float projectileSpeed = 80;

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
        return TurretType.Crossbow;
    }

    public override float GetTurretCost()
    {
        return worth;
    }

    override protected void Attack()
    {
        Debug.Log("MCDIE");
    }
}
