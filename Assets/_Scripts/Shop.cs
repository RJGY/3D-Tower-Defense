using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public delegate void PurchaseTurret();
    public event PurchaseTurret OnPurchase;
    public event PurchaseTurret StoppedPurchase;
    private Node currentNode;
    #region Singleton
    public static Shop Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one shop");
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    private void Start()
    {
        CloseShop();
    }
    public void SelectArcherTurret()
    {
        BuildManager.Instance.SetTurretToBuild(Turrets.TurretType.Archer);
    }

    public void SelectBombTurret()
    {

    }

    public void SelectSorcererTower()
    {

    }

    public void SelectIceTower()
    {

    }

    public void SelectTower()
    {
        BuildManager.Instance.SetTurretToBuild(Turrets.TurretType.Undefined);
    }
    
    public void PurchaseTower()
    {
        if (OnPurchase != null)
        {
            OnPurchase();
            Debug.Log("Turret Purchased");
        }
    }
    public void OpenShop(Node node)
    {
        currentNode = node;
        gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        if (StoppedPurchase != null)
        {
            StoppedPurchase();
        }
        else
        {
            Debug.Log("STOPPED PURCHASE");
        }
        currentNode = null;
        gameObject.SetActive(false);
    }
}
