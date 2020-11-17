﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : Entity
{
    #region Variables
    [Header("NavMesh Properties")]
    private NavMeshAgent agent;
    private Vector2 lastPosition;


    [Header("Combat Properties")]
    private Enemy currentEnemy;
    private Animator animator;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        // GetComponents in Awake.
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        
    }

    

    void OnDisable()
    {
        MouseManager.Instance.PlayerMoved -= MovePlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        MouseManager.Instance.PlayerMoved += MovePlayer;
        MouseManager.Instance.PlayerAttacked += Attack;

        AssignStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            InstantRotation();
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (currentEnemy != null)
        {
            Attack(currentEnemy);
        }
        else if (currentEnemy == null && new Vector2(agent.destination.x, agent.destination.z) != lastPosition)
        {
            StopAttackingNullEnemy();
        }

        if (animator.GetBool("isAttacking"))
        {
            // Player stops to attack.
            agent.SetDestination(transform.position);
        }
    }

    #endregion

    #region Functions

    private void AssignStats2()
    {
        maxHealth = 100;
        health = maxHealth;
        physicalDamage = 1;
        magicDamage = 1;
        attackRange = 1;
        attackSpeed = 0.5f;
        agent.updateRotation = false;
        canAttack = true;
    }

    void StopAttackingNullEnemy()
    {
        Debug.Log(agent.destination);
        Debug.Log(lastPosition);
        agent.SetDestination(transform.position);
        lastPosition = new Vector2(transform.position.x, transform.position.z);
    }

    void InstantRotation()
    {
        var _lookRotation = Quaternion.LookRotation(agent.velocity.normalized);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, 8);

        
    }

    private void Attack(Enemy enemy)
    {

        if (currentEnemy == null)
        {
            currentEnemy = enemy;
        }

        // Player is in range of enemy
        if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            Debug.Log("I am in range of the enemy and i am attempting to attack");
            if (canAttack)
            {
                // Player does animation to attack
                animator.SetBool("isAttacking", true);

                // Player hits enemy.
                Debug.Log("The player has attacked " + currentEnemy.name);
                currentEnemy.TakeDamage(physicalDamage, magicDamage);
                canAttack = false;

                // Player attack cooldown
                StartCoroutine(AttackCooldown());
            }
            else
            {
                // Still in attack cooldown

            }

        }
        else
        {
            // Player pathfind to enemy
            if (agent.destination != currentEnemy.transform.position)
            {
                agent.SetDestination(currentEnemy.transform.position);
            }
        }
    }

    private void MovePlayer(Vector3 position)
    {
        agent.SetDestination(position);
        lastPosition = new Vector2(agent.destination.x, agent.destination.z);
        currentEnemy = null;
    }

    #endregion
}
