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
    private float health;
    private float maxHealth;
    private float physDamage;
    private float magicDamage;
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
    }

    #endregion

    #region Functions

    private void AssignStats()
    {
        maxHealth = 100;
        health = maxHealth;
        physDamage = 5;
        magicDamage = 5;
        agent.updateRotation = false;
    }

    void InstantRotation()
    {
        var _lookRotation = Quaternion.LookRotation(agent.velocity.normalized);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, 8);
    }

    public void Attack(Enemy enemy)
    {
        // Assign
        enemy.TakeDamage(physDamage, magicDamage);
    }

    public void TakeDamage()
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
}
