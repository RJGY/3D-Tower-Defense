using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : MonoBehaviour
{
    #region Events

    #endregion

    #region Variables

    [Header("Turret Combat Variables")]
    protected float health;
    protected float attackRange;
    protected float attackSpeed; // Measured in attacks per second.
    protected float attackDamage;
    protected float magicDamage;
    protected float pureDamage;
    protected float armourPenetration;
    protected float magicResistPenetration;
    protected float projectileSpeed;
    protected TurretType turretType;
    protected AttackType attackType;
    protected float armour;
    protected float magicresist;
    protected float worth;
    protected bool buffed;
    protected float currentBuffValue;

    [Header("Turret Effect Varaibles")]
    protected Outline outline;

    public enum TurretType
    {
        Sorcerer,
        Archer,
        Bomb,
        Ice,
        Crossbow,
        Hellfire,
        Buff,
        Debuff,
        Dark,
        Undefined
    }

    public enum AttackType
    {
        First,
        Last,
        Closest,
        Strongest
    }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        MouseManager.Instance.TowerSelected += OutlineEffectOn;
        MouseManager.Instance.TowerDeselected += OutlineEffectOff;
    }

    void OnDisable()
    {
        MouseManager.Instance.TowerSelected -= OutlineEffectOn;
        MouseManager.Instance.TowerDeselected -= OutlineEffectOff;
    }

    #endregion

    #region Functions

    protected void OutlineEffectOn(Vector3 position)
    {
        if (position == transform.position)
        {
            outline.eraseRenderer = false;
        }
    }

    protected void OutlineEffectOff()
    {
        outline.eraseRenderer = true;
    }

    protected bool EnemyInRange()
    {
        return false;
    }

    protected Transform TargetClosest()
    {
        Transform closestEnemy = null;

        return closestEnemy;
    }

    protected Transform TargetFirst()
    {
        Transform firstEnemy = null;
        
        return firstEnemy;
        // Transform must be in range && transform distance to waypoint is the lowest.
    }

    protected Transform TargetStrongest()
    {
        // Transform must be in range && total health * defense is highest.
        Transform strongestEnemy = null;
        
        return strongestEnemy;
    }

    protected Transform TargetLast()
    {
        // Transform must be in range && transform distance to waypoint is the highest.
        Transform lastEnemy = null;
        
        return lastEnemy;
    }

    protected void Attack()
    {
        // Fix
    }

    protected void TakeDamage(float pDamage, float mDamage, float tDamage, float pPen, float mPen)
    {

    }
    #endregion

    #region Coroutines

    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1 / attackSpeed);
    }

    #endregion
}
