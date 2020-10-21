using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("NavMesh Properties")]
    private NavMeshAgent agent;


    [Header("Combat Properties")]
    private Enemy currentEnemy;
    private float health;
    private float maxHealth;
    private float physDamage;
    private float magicDamage;
    private float attackRange;
    private float attackRate;
    private bool canAttack;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        // GetComponents in Awake.
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        MouseManager.Instance.PlayerMoved += MovePlayer;
        MouseManager.Instance.PlayerAttacked += Attack;
    }

    

    void OnDisable()
    {
        MouseManager.Instance.PlayerMoved -= MovePlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        AssignStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            InstantRotation();
        }

        if (currentEnemy != null)
        {
            Attack(currentEnemy);
        }
    }

    #endregion

    #region Functions

    private void AssignStats()
    {
        maxHealth = 100;
        health = maxHealth;
        physDamage = 1;
        magicDamage = 1;
        attackRange = 1;
        agent.updateRotation = false;
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

        // Player pathfind to enemy
        if (agent.destination != currentEnemy.transform.position)
        {
            agent.SetDestination(currentEnemy.transform.position);
        }

        // Player is in range of enemy
        if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
        {
            // Player does animation to attack

            // Player hits enemy.
            Debug.Log("The player has attacked " + currentEnemy.name);
            currentEnemy.TakeDamage(physDamage, magicDamage);

            // Player attack cooldown

        }
    }

    public void TakeDamage(float physDamage, float magicDamage)
    {


        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void MovePlayer(Vector3 position)
    {
        agent.SetDestination(position);
    }

    #endregion

    private IEnumerator AttackCooldown()
    {
        if (!canAttack)
        {
            yield return new WaitForSeconds(attackRate);
            canAttack = true;
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }

    }
}
