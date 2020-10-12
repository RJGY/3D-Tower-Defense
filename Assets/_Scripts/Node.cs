using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class Node : MonoBehaviour
{
    #region Variables

    public Color hoverColor;

    private Turrets _childTurret;
    private Renderer _rend;
    private Color startColor;
    [SerializeField] private LayerMask nodeLayer;

    #endregion

    #region Monobehavior

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        startColor = _rend.material.color;

        

        
    }

    void Update()
    {
        var mouse = Mouse.current;

        if (mouse != null)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                if (Physics.Raycast(ray, out hit, nodeLayer))
                {
                    if (hit.transform == transform)
                    {
                        _childTurret = Instantiate(BuildManager.instance.TurretToBuild, transform);
                    }
                }
            }
        }


    }

    #endregion
}
