using System.Collections;
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
    [SerializeField] private Enemy currentEnemy;
    private Animator animator;
    private float moveSpeed;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float projectileSpeed;

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
        if (stoppedToAttack)
        {
            // Player stops to attack.
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
        else if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
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
        else if (new Vector2(agent.destination.x, agent.destination.z) != lastPosition)
        {
            StopAttackingNullEnemy();
        }

        
    }

    #endregion

    #region Functions

    private new void AssignStats()
    {
        base.AssignStats();
        maxHealth = 100;
        health = maxHealth;
        physicalDamage = 5;
        magicDamage = 1;
        attackRange = 2;
        attackSpeed = 0.5f;
        agent.updateRotation = false;
        canAttack = true;
        moveSpeed = 3.5f;
        agent.speed = moveSpeed;
        animator.SetFloat("attackSpeed", attackSpeed);
        animator.SetFloat("moveSpeed", Mathf.Sqrt(moveSpeed / 3.5f));
        isMelee = false;
    }

    private void StopAttackingNullEnemy()
    {
        agent.SetDestination(transform.position);
        lastPosition = new Vector2(transform.position.x, transform.position.z);
    }

    private void InstantRotation()
    {
        // Get Quaternion to look at based on movement.
        var _lookRotation = Quaternion.LookRotation(agent.velocity.normalized);

        // Look towards _lookRotation.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, 8);
    }


    private void MeleeToggle()
    {
        // Toggle melee weapon collider to deal damage to enemies.
    }

    private void SpawnAttackProjectile()
    {
        // Spawn bullet and shoot at enemy
        Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.AssignStats(physicalDamage, magicDamage, enemyLayer, currentEnemy.transform, projectileSpeed);
    }

    private void CanMove()
    {
        stoppedToAttack = false;
    }

    private void CanAttack()
    {
        canAttack = true;
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
            if (canAttack)
            {
                

                // Set animator to attack.
                canAttack = false;
                animator.SetTrigger("isAttacking");
                stoppedToAttack = true;
            }
            else if(!canAttack && !stoppedToAttack)
            {
                // Still in attack cooldown.
                // Get Quaternion to look at based on movement.
                var _lookRotation = Quaternion.LookRotation(-currentEnemy.transform.position);

                // Look towards _lookRotation.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, 8);
            }

        }
        else
        {
            // Player pathfind to enemy
            agent.SetDestination(currentEnemy.transform.position);
        }
    }

    private void MovePlayer(Vector3 position)
    {
        if (stoppedToAttack)
        {
            return;
        }

        agent.SetDestination(position);
        lastPosition = new Vector2(agent.destination.x, agent.destination.z);
        currentEnemy = null;
    }

    #endregion
}
