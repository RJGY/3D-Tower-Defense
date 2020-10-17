using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables
    private NavMeshAgent agent;
    [SerializeField] private LayerMask groundLayer;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        // GetComponents in Awake.
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            MoveToPoint();
        }

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            InstantRotation();
        }

    }

    #endregion

    #region Functions

    void InstantRotation()
    {
        var _lookRotation = Quaternion.LookRotation(agent.velocity.normalized);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, 8);
    }

    void MoveToPoint()
    {
        var mouse = Mouse.current;
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            agent.SetDestination(hit.point);
        }
    }
    #endregion
}
