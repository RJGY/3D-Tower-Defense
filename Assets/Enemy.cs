using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("NavMeshAgent Properties")]
    [Space]
    public NavMeshAgent agent;
    public Transform wayPointParent;
    public Transform[] points;
    public int currentWayPoint;
    public float wayPointDistance = 1.5f;
    public float agentStoppingDistance = 0.5f;
    public float agentAngularSpeed = 400;
    public float agentAcceleration = 40;
    
    
    [Header("Enemy Statistics")]
    public float health;
    public float moveSpeed;
    public float armour;
    // These enums are for assigning stats on start.
    public enum EnemyType
    {
        Slow,
        Fast,
    }

    public enum EnemyArt
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
        agent = GetComponent<NavMeshAgent>();
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
        }
        else
        {
            Debug.LogError("NO AGENT ATTACHED");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GoToEnd();
        // Need a gamemanager to check if the enemy is slowed or not.
        // Make slowing enemies an event.
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
                Debug.Log("I SHOULD DIE HERE");
                LoseALife();
            }
        }
    }

    void CheckIfSlowed()
    {

    }

    void LoseALife()
    {
        // Player should lose a life because the enemy reached the end of the level.
        // Make this an event.
        Destroy(gameObject);
    }
    #endregion
}
