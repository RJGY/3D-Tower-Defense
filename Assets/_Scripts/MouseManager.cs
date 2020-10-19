using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseManager : MonoBehaviour
{
    #region Events
    public delegate void MovePlayer(Vector3 position);
    public event MovePlayer PlayerMoved;
    public event MovePlayer BuiltTurret;
    public event MovePlayer NodeSelected;
    public delegate void AttackPlayer(Enemy enemy);
    public event AttackPlayer PlayerAttacked;
    public delegate void NodeSelect();
    public event NodeSelect NodeDeselected;

    #endregion

    #region Variables
    private Mouse mouse;
    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    #endregion


    #region Singleton
    public static MouseManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple MouseManagers in scene.");
        }

    }
    #endregion

    private void Start()
    {
        mouse = Mouse.current;
    }

    void Update()
    {
        CheckIfNodeMouseHover();

        if (mouse.rightButton.wasPressedThisFrame)
        {
            if (!PlayerAttack())
            {
                MoveToPoint();
            }
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            BuildTurret();
        }
    }

    
    void CheckIfNodeMouseHover()
    {
        NodeDeselected?.Invoke();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeLayer))
        {
            NodeSelected?.Invoke(hit.transform.position);
        }
    }

    void MoveToPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            PlayerMoved?.Invoke(hit.point);
        }
    }

    void BuildTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeLayer))
        {
            BuiltTurret?.Invoke(hit.transform.position);
        }
    }

    private bool PlayerAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            PlayerAttacked?.Invoke(hit.transform.GetComponent<Enemy>());
            return true;
        }

        return false;
    }
}
