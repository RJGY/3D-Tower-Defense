using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTurret : Turrets
{
    // BASE STATS
    protected new float worth = 50;
    protected new float attackDamage = 8;
    protected new float magicDamage = 0;
    protected new float pureDamage = 0;
    protected new float armourPenetration = 50;
    protected new float magicResistPenetration = 0;
    protected new float splashRange = -1;
    protected new float attackRange = 12;
    protected new float attackSpeed = 2;
    protected new float projectileSpeed = 40;

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
        return TurretType.Archer;
    }
    
    public override float GetTurretCost()
    {
        return worth;
    }
    
}
