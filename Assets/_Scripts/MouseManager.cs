using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseManager : MonoBehaviour
{
    #region Variables
    private Mouse mouse;
    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] private LayerMask groundLayer;
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
            Debug.LogError("Multiple GameManagers in scene.");
        }

    }
    #endregion

    private void Start()
    {
        mouse = Mouse.current;
    }

    void Update()
    {
        
    }

    void CheckIfNodeMouseHover()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeLayer))
        {
            if (hit.transform == transform)
            {
                    
            }

            else
            {
                    
            }
        }
    }

    public void MoveToPoint()
    {
        var mouse = Mouse.current;
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            
        }
    }
}
