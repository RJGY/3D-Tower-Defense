using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Node : MonoBehaviour
{
    #region Variables

    private Turrets _childTurret;
    private Outline outline;
    
    private Mouse mouse;
    #endregion

    #region Monobehavior

    private void Awake()
    {
        outline = GetComponent<Outline>();
        mouse = Mouse.current;
    }

    // Start is called before the first frame update
    private void Start()
    {
        outline.eraseRenderer = true;
    }

    void Update()
    {
        if (_childTurret == null)
        {
            
        }
    }

    #endregion

    
}
