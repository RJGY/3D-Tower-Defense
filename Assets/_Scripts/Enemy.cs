using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Events

    #endregion 

    #region Variables
    [Header("NavMeshAgent Properties")]
    private NavMeshAgent agent;
    [SerializeField] private LayerMask towerLayer;
    private CapsuleCollider capsuleCollider;
    private Transform endPathTransform;

    [Header("Enemy Statistics")]
    private EnemyType enemyType;
    private EnemySpecies enemyArt;
    [SerializeField] private float attackRange;
    private float attackDamage;
    private float attackRate;
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
        capsuleCollider = GetComponent<CapsuleCollider>();
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

        // Call Coroutines.
        StartCoroutine(CheckForEnd());
        StartCoroutine(CheckPath());
    }


    // Update is called once per frame
    private void Update()
    {

            
    }
    #endregion

    #region Functions
    private void GoToEnd()
    {
        // Go towards the end point.
        agent.SetDestination(endPathTransform.position);
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


    private void AttackTurret()
    {
        // Find closest turret
        targetedTurret = FindClosestTower();

        // Pathfind towards turret
        Debug.Log("Pathing towards " + targetedTurret.name, targetedTurret);
        agent.SetDestination(targetedTurret.transform.position);

        // Attack closest turret.
        if (Vector3.Distance(transform.position, targetedTurret.position) < attackRange)
        {
            Debug.Log("I have attacked " + targetedTurret.name, targetedTurret);
            Destroy(targetedTurret.gameObject);
        }
    }

    private void AttackPlayer()
    {
        return;
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
        Destroy(gameObject);
    }
    #endregion

    #region Coroutines

    private IEnumerator CheckForEnd()
    {
        // Every 0.2 seconds, check if we are colliding with the end.
        yield return new WaitForSeconds(0.2f);
        // Distance check.
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(endPathTransform.position.x, endPathTransform.position.z)) <= agent.radius)
            OnReachEnd();
        StartCoroutine(CheckForEnd());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackRate);
    }

    private IEnumerator CheckPath()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (agent.hasPath)
        {
            // Pathfind towards end.
            Debug.Log("Going to end");
            GoToEnd();
        }
        else if (!agent.hasPath && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(endPathTransform.position.x, endPathTransform.position.z)) >= agent.radius)
        {
            // Attack Closest Turret
            Debug.Log("Attacking");
            AttackTurret();
        }

        StartCoroutine(CheckPath());
    }
    #endregion
}
