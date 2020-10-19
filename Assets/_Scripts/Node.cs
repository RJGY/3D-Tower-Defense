using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    #region Variables

    private Turrets childTurret;
    private Outline outline;
    #endregion

    #region Monobehavior

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        outline.eraseRenderer = true;
    }

    private void OnEnable()
    {
        MouseManager.Instance.BuiltTurret += BuildTurret;
        MouseManager.Instance.NodeDeselected += DeselectNode;
        MouseManager.Instance.NodeSelected += NodeSelected;
    }



    private void OnDisable()
    {
        MouseManager.Instance.BuiltTurret -= BuildTurret;
        MouseManager.Instance.NodeDeselected -= DeselectNode;
        MouseManager.Instance.NodeSelected -= NodeSelected;
    }

    #endregion
    private void BuildTurret(Vector3 position)
    {
        if (childTurret == null)
        {
            if (position == transform.position)
            {
                childTurret = Instantiate(BuildManager.instance.TurretToBuild, transform);
                DeselectNode();
            }
        }
    }

    private void NodeSelected(Vector3 position)
    {
        if (position == transform.position && childTurret == null)
        {
            outline.eraseRenderer = false;
        }
    }

    private void DeselectNode()
    {
        if (outline.eraseRenderer == false)
        {
            outline.eraseRenderer = true;
        }
    }

}
