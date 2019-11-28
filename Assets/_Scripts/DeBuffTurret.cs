using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffTurret : Turrets
{
    // THIS DUDE NEEDS NEW ATTACK FUNCTION
    // THis dude doesnt need a projectile
    // BASE STATS
    public new float Worth = 500000000;
    protected new float attackDamage = 1.5f;
    protected new float magicDamage = 0;
    protected new float pureDamage = 0;
    protected new float armourPenetration = 0;
    protected new float magicResistPenetration = 0;
    protected new float splashRange = -1;
    protected new float attackRange = 15;
    protected new float attackSpeed = 1;
    protected new float projectileSpeed = 1000;
    protected new Projectile projectilePrefab = null;
    private List<Turrets> turrets;
    [SerializeField]
    private NodeHolder nodeHolder;
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
        return TurretType.Debuff;
    }

    public override float GetTurretCost()
    {
        return Worth;
    }
}
