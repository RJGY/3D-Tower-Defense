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
    [SerializeField] private LayerMask nodeLayer;
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
            if (mouse != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeLayer))
                {
                    if (hit.transform == transform)
                    {
                        outline.eraseRenderer = false;

                        if (mouse.leftButton.wasPressedThisFrame)
                        {
                            _childTurret = Instantiate(BuildManager.instance.TurretToBuild, transform);
                            outline.eraseRenderer = true;
                        }
                    }

                    else
                    {
                        outline.eraseRenderer = true;
                    }
                }

            }
        }
    }

    #endregion
}
