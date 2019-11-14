using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Variables
    public delegate void LostALife(int lives, Enemy enemy);
    public event LostALife OnLifeLost;
    public delegate void EnemyKilled(Enemy enemy);
    public event EnemyKilled JustDied;
    public delegate void SendHealth(float health);
    public event SendHealth OnHealthSent;

    [Header("NavMeshAgent Properties")]
    private NavMeshAgent agent;
    private Transform wayPointParent;
    private Transform[] points;
    private int currentWayPoint;
    private float wayPointDistance = 1.5f;
    private float agentStoppingDistance = 0f;
    private float agentAngularSpeed = 400;
    private float agentAcceleration = 40;
    private bool agentCanMove = true;

    [Header("Enemy Statistics")]
    private EnemyType enemyType;
    private EnemySpecies enemyArt;
    private GameManager.Difficulty difficulty;
    private int[] firstHealthSpawns = { 4, 6, 8, 11 };
    private float maxHealth;
    private float health;
    private float moveSpeed;
    private float armour;
    private float magicResist;
    private int livesWorth;
    private float goldReward;

    [Header("EnemyUI")]
    private Image healthImage;


    // These enums are for assigning stats on start.
    public enum EnemyType
    {
        Slow,
        Fast,
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
        healthImage = FindObjectOfType<HealthBar>().GetComponent<Image>();
        agent = GetComponent<NavMeshAgent>();
        wayPointParent = FindObjectOfType<WaypointParent>().GetComponent<Transform>();

        if (wayPointParent != null)
        {
            points = wayPointParent.GetComponentsInChildren<Transform>();
        }
        else
        {
            Debug.LogError("NO WAYPOINT PARENT ATTACHED");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (agent != null)
        {
            agent.acceleration = agentAcceleration;
            agent.stoppingDistance = agentStoppingDistance;
            agent.angularSpeed = agentAngularSpeed;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            agent.autoBraking = false;
            currentWayPoint = 1;
            GameManager.Instance.OnGameEnded += Instance_OnGameEnded;
            // The assigning of movespeed, health and armour/mr goes down here.
            livesWorth = 1; // TEMP, DELETE LATER
            maxHealth = 10;
            health = maxHealth;
        }
        else
        {
            Debug.LogError("NO AGENT ATTACHED");
        }

        StartCoroutine(UpdateHealthBar());
    }

    private void Instance_OnGameEnded()
    {
        agentCanMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agentCanMove)
        {
            GoToEnd();
        }
        else
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
            }
        }
        // Need a gamemanager to check if the enemy is slowed or not.
        // Make slowing enemies an event.
    }
    #endregion

    #region Public Functions
    public void SplashSubscribeToTurret(Projectile projectile)
    {
        projectile.SendBackToEnemy += Enemy_OnEnemyHit;
    }

    public float GetHealth()
    {
        return health;
    }
    #endregion

    #region Functions
    void GoToEnd()
    {
        if (points.Length < 1)
        {
            Debug.LogError("No waypoints assigned.");
            return;
        }

        agent.SetDestination(points[currentWayPoint].position);

        if (Vector3.Distance(transform.position, points[currentWayPoint].position) < wayPointDistance)
        {
            if (currentWayPoint < points.Length - 1)
            {
                currentWayPoint++;
            }
            else
            {
                LoseALife();
            }
        }
    }

    void LoseALife()
    {
        // Player should lose a life because the enemy reached the end of the level.
        if (OnLifeLost != null)
        {
            GameManager.Instance.OnGameEnded -= Instance_OnGameEnded;
            OnLifeLost(livesWorth, this);
        }
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            other.GetComponent<Projectile>().SendBackToEnemy += Enemy_OnEnemyHit;
        }
    }

    

    private void Enemy_OnEnemyHit(Turrets turret)
    {
        turret.OnTookDamage += TakeDamage;
    }

    void TakeDamage(float attackDamage, float armourPenetration, float magicDamage, float magicResistPenetration, float pureDamage, Turrets turret, Projectile projectile)
    {
        float damageTaken = pureDamage;
        float effectiveArmour = armour - armourPenetration;
        float effectiveMagicResist = magicResist - magicResistPenetration;
        damageTaken += attackDamage * (1 - (effectiveArmour / (effectiveArmour + 100)));
        damageTaken += magicDamage * (1 - (effectiveMagicResist / (effectiveMagicResist + 100)));
        health -= damageTaken;

        turret.OnTookDamage -= TakeDamage;
        projectile.SendBackToEnemy -= Enemy_OnEnemyHit;

        if (health <= 0)
        {
            StartCoroutine(Die());
        }

        StartCoroutine(UpdateHealthBar());
    }

    IEnumerator Die()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.OnGameEnded -= Instance_OnGameEnded;

        if (JustDied != null)
        {
            JustDied(this);
        }

        Destroy(gameObject);
    }

    IEnumerator UpdateHealthBar()
    {
        yield return new WaitForEndOfFrame();
        float amount = Mathf.Clamp01(health / maxHealth);
        healthImage.fillAmount = amount;
    }
    #endregion
}
