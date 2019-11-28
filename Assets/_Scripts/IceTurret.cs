using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTurret : Turrets
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
        magicDamage = 5;
        pureDamage = 2;
        armourPenetration = 0;
        magicResistPenetration = 80;
        splashRange = 2;
        attackRange = 12;
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
        return TurretType.Ice;
    }

    public override float GetTurretCost()
    {
        return Worth;
    }

    override protected void Attack()
    {
        Debug.Log("MCDIE");
    }
}
