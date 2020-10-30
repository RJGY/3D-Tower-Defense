using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseManager : MonoBehaviour
{
    #region Events
    public delegate void Position(Vector3 position);
    public event Position PlayerMoved;
    public event Position BuiltTurret;
    public event Position NodeSelected;
    public event Position EnemySelected;
    public event Position TowerSelected;
    public delegate void AttackPlayer(Enemy enemy);
    public event AttackPlayer PlayerAttacked;
    public delegate void Deselect();
    public event Deselect NodeDeselected;
    public event Deselect EnemyDeselected;
    public event Deselect TowerDeselected;


    #endregion

    #region Variables
    private Mouse mouse;
    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask towerLayer;
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
        if (CheckIfEnemyHover())
        {
            NodeDeselected?.Invoke();
            Debug.Log("Hovering enemy");
        }
        else if (CheckIfTowerHover())
        {
            NodeDeselected?.Invoke();
            Debug.Log("Hovering Tower");
        }
        else
        {
            CheckIfNodeMouseHover();
            Debug.Log("Hovering Node");
        }
        

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

    bool CheckIfEnemyHover()
    {
        EnemyDeselected?.Invoke();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
        {
            EnemySelected?.Invoke(hit.transform.position);
            return true;
        }

        return false;
    }

    bool CheckIfTowerHover()
    {
        TowerDeselected?.Invoke();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, towerLayer))
        {
            TowerSelected?.Invoke(hit.transform.position);
            return true;
        }

        return false;
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
