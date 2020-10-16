using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Btkalman.Util;

public class Enemy : MonoBehaviour
{
    #region Events

    #endregion 

    #region Variables
    [Header("NavMeshAgent Properties")]
    private NavMeshAgent agent;
    [SerializeField] private LayerMask towerLayer;
    private Transform endPathTransform;

    [Header("Enemy Statistics")]
    private EnemyType enemyType;
    private EnemySpecies enemyArt;

    [Header("Enemy Attack Variables")]
    [SerializeField] private float attackRange;
    private float attackDamage;
    private float attackRate;
    private bool canAttack;


    private float maxHealth;
    private float health;
    private float moveSpeed;
    private float armour;
    private float magicResist;
    private int livesWorth;
    private float goldReward;
    private Transform targetedTurret;

    [Header("Enemy UI")]
    private Image healthImage;

    // These enums are for assigning stats on start.
    public enum EnemyType
    {
        Slow,
        Fast,
        MagicImmune,
        PhysicalImmune,
        DamageReduction
    }

    public enum EnemySpecies
    {
        Orc,
        Goblin,
        Zombie,
        Skeleton,
        DarkKnight,
        Wizard
    }
    #endregion

    #region MonoBehavior
    private void Awake()
    {
        // GetComponents in Awake.
        agent = GetComponent<NavMeshAgent>();
    }

    // OnEnable is called before Start
    private void OnEnable()
    {
        // Subscribe event.
        EnemySpawner.instance.EnemySpawned += EnemySpawner_EnemySpawned;
    }

    private void OnDisable()
    {
        // This is just incase the event does not unsubscribe when it triggers.
        EnemySpawner.instance.EnemySpawned -= EnemySpawner_EnemySpawned;
    }

    private void OnDestroy()
    {
        // This is just incase the units coroutines somehow persist after destroy.
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Call functions.
        GoToEnd();
        AssignStats();

        // Call Coroutines.
        StartCoroutine(CheckPath());
    }


    // Update is called once per frame
    private void Update()
    {
        CheckPath();
    }
    #endregion

    #region Functions

    private void GoToEnd()
    {
        // Go towards the end point.
        if (agent.destination != endPathTransform.position)
        {
            agent.SetDestination(endPathTransform.position);
        }
        else
        {
            Debug.LogError("Agent Destination not set.");
        }
    }

    private void AssignStats()
    {
        // Assign Variables
        // Put this into a switch statement later.
        attackRate = 0.2f;
        attackRange = 1.5f;
        attackDamage = 10f;
        maxHealth = 10;
        health = maxHealth;
        moveSpeed = 3.5f;
        agent.speed = moveSpeed;
        canAttack = true;
    }

    private void EnemySpawner_EnemySpawned(Transform endPathTransform)
    {
        // Assign the end point via Event.
        this.endPathTransform = endPathTransform;
        // Unsubscribe event.
        EnemySpawner.instance.EnemySpawned -= EnemySpawner_EnemySpawned;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Mathf.Min(agent.radius * 10, agent.radius + 10));
    }


    private void Attack()
    {
        // Find closest turret
        if (targetedTurret == null)
            targetedTurret = FindClosestTower();

        // Pathfind towards turret
        if (agent.destination != targetedTurret.transform.position)
        {
            agent.SetDestination(targetedTurret.transform.position);
        }
        
        // Attack closest turret.
        if (Vector3.Distance(transform.position, targetedTurret.position) < attackRange)
        {
            Debug.Log("I am attempting to attack " + targetedTurret.name);
            if (canAttack)
            {
                Debug.Log("I have attacked " + targetedTurret.name, targetedTurret);

                // Deal Damage
                // Damage should be dealt through animation event.
                Debug.Log("I dealt " + attackDamage + " damage.");
                Destroy(targetedTurret.gameObject);

                // Attack reset.

                canAttack = false;
                StartCoroutine(AttackCooldown());
                GoToEnd();
            }
        }
    }       

    private Transform FindClosestTower()
    {
        // Define our search radius.
        float searchRadius = agent.radius + 8; 
        // Define turret variable
        Transform closestTurret = null;
        Collider[] turrets = Physics.OverlapSphere(transform.position, searchRadius, towerLayer);
        foreach (Collider turret in turrets)
        {
            if (closestTurret == null)
                closestTurret = turret.transform;
            else
            {
                Transform currentTurretTransform = turret.transform;
                if (Vector3.Distance(currentTurretTransform.position, transform.position) < Vector3.Distance(closestTurret.position, transform.position))
                    closestTurret = turret.transform;
            }
        }

        return closestTurret;
    }

    private void OnReachEnd()
    {
        // Link to GameManager to lose lives.

        // Destroy myself
        Destroy(gameObject);
    }

    public void TakeDamage(float physDamage, float magicDamage)
    {
        // This needs to be an event later on.
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Give gold reward to gamemanager.
        
        // Destroy gameobject
        Destroy(gameObject);
    }
    #endregion

    #region Coroutines
    private IEnumerator CheckPath()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("Checking Path");

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(endPathTransform.position.x, endPathTransform.position.z)) <= agent.radius)
        {
            // End reached.
            OnReachEnd();
        }
        else if (!agent.pathPending && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            // Attack
            Attack();
        }

        StartCoroutine(CheckPath());
    }

    private IEnumerator AttackCooldown()
    {
        if (!canAttack)
        {
            yield return new WaitForSeconds(1 / attackRate);
            canAttack = true;
        }
        
        yield return new WaitForEndOfFrame();
    }

    
    #endregion
}
