using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    #region Variables

    public Color hoverColor;

    private Turrets _childTurret;
    private Renderer _rend;
    private Color startColor;

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

    private void OnMouseDown()
    {
        if (_childTurret != null)
            return;

        _childTurret = Instantiate(BuildManager.instance.GetTurretToBuild(), transform);
    }

    private void OnMouseEnter()
    {
        _rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        _rend.material.color = startColor;
    }

    #endregion
}
