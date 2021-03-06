﻿using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrets : Entity
{
    #region Events

    #endregion

    #region Variables

    [Header("Turret Variables")]
    protected TurretType turretType;
    protected AttackType attackType;
    protected bool isBuffed;
    protected float currentBuffValue;

    [Header("Turret GameManager Variables")]
    protected float worth;
    

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

    #endregion

    #region Coroutines

    #endregion
}
