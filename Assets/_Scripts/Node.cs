using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color defaultColor;
    private Renderer rend;
    private Turrets turret;
    private Vector3 offset;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        turret = null;
        defaultColor = rend.material.color;
        offset = new Vector3(0, GetComponent<Transform>().localScale.y * 0.5f, 0);
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseDown()
    {
        if (turret != null)
        {
            Debug.Log("YOU CANT BUILD A TURRET, THERES A TURRET HERE ALREADY");
            return;
        }
        
        turret = Instantiate(BuildManager.Instance.GetTurretToBuild(), transform.position + offset, transform.rotation, transform);
    }

    private void OnMouseExit()
    {
        rend.material.color = defaultColor;
    }
}
