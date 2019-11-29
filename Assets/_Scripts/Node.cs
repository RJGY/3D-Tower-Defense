using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color defaultColor;
    public Color occupiedColor;
    private Renderer rend;
    private Turrets turret;
    private Vector3 offset;
    private bool selected;
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
        if (turret == null)
        {
            rend.material.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (turret == null && !selected)
        {
            rend.material.color = defaultColor;
        }
    }

    private void OnMouseDown()
    {
        selected = true;
        if (turret != null)
        {
            // OPEN UPGRADE MANAGER
            UpgradeManager.Instance.OpenUpgrade(turret);
            // NEED EVENTS
            return;
        }

        // TODO - OPEN THE SHOP
        Shop.Instance.OpenShop(this);
        Shop.Instance.OnPurchase += BuildTurret;
        Shop.Instance.StoppedPurchase += BuildManager_Instance_StoppedPurchase;
    }

    private void BuildManager_Instance_StoppedPurchase()
    {
        Shop.Instance.OnPurchase -= BuildTurret;
        Shop.Instance.StoppedPurchase -= BuildManager_Instance_StoppedPurchase;
        selected = false;
        rend.material.color = defaultColor;
    }

    private void BuildTurret()
    {
        Shop.Instance.OnPurchase -= BuildTurret;
        Shop.Instance.StoppedPurchase -= BuildManager_Instance_StoppedPurchase;
        if (turret == null)
        {
            turret = Instantiate(BuildManager.Instance.GetTurretToBuild(), transform.position + offset, transform.rotation, transform);
            turret.name = BuildManager.Instance.GetTurretToBuild().ToString();
            rend.material.color = occupiedColor;
        }
        else
        {
            Debug.LogError("YOU CANT BUILD A TURRET, THERES A TURRET HERE ALREADY");
        }
    }

    public void RemoveTower()
    {
        turret = null;
        rend.material.color = defaultColor;
    }
}
