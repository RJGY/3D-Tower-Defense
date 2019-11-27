using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public delegate void PurchaseTurret();
    public event PurchaseTurret OnPurchase;
    public event PurchaseTurret StoppedPurchase;
    private Node currentNode;
    public Button purchaseButton;
    private Vector2 screen;
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
        screen.x = Screen.width / 16;
        screen.y = Screen.height / 9;
    }

    public void SelectTower(int turretType)
    {
        BuildManager.Instance.SetTurretToBuild((Turrets.TurretType)turretType);
        purchaseButton.gameObject.SetActive(true);
    }

    public void PurchaseTower()
    {
        // if money grater than cost of tower, buy the tower
        if ()
        {

        }
        else
        {
            return;
        }
        if (OnPurchase != null)
        {
            OnPurchase();
            Debug.Log("Turret Purchased");
        }
        CloseShop();
    }
    public void OpenShop(Node node)
    {
        if (currentNode == null)
        {
            currentNode = node;
            gameObject.SetActive(true);
        }
        else
        {
            if (StoppedPurchase != null)
            {
                StoppedPurchase();
            }
            else
            {
                Debug.Log("STOPPED PURCHASE");
            }
            currentNode = node;
        }
        purchaseButton.gameObject.SetActive(false);
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
        purchaseButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
