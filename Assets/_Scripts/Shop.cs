using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private void Start()
    {
        CloseShop();
    }
    public void PurchaseStandardTurret()
    {

    }

    public void PurchaseAnotherTurret()
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
