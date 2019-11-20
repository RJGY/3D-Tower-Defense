using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private void Start()
    {
        CloseShop();
    }
    public void PurchaseArcherTurret()
    {
        
    }

    public void PurchaseBombTurret()
    {

    }

    public void PurchaseMagicTower()
    {

    }

    public void PurchaseIceTower()
    {

    }

    public void PurchaseTower()
    {

    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
}
