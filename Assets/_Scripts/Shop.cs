using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public delegate void PurchaseTurret();
    public delegate void PayForTurret(float cost);
    public event PayForTurret OnPay;
    public event PurchaseTurret OnPurchase;
    public event PurchaseTurret StoppedPurchase;

    private float snapshotOfMoney;
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

    #region Functions

    public void SelectTower(int turretType)
    {
        BuildManager.Instance.SetTurretToBuild((Turrets.TurretType)turretType);
        ShowButton(true);
    }

    public void PurchaseTower()
    {
        if (GameManager.Instance.GetGold() >= BuildManager.Instance.GetTurretPrice())
        {
            if (OnPurchase != null)
            {
                OnPurchase();
                OnPay(BuildManager.Instance.GetTurretPrice());
                Debug.Log("Turret Purchased");
            }
            else
            {
                Debug.Log("No node subscribed to OnPurchased");
            }
            CloseShop();
        }
        else
        {
            Debug.Log("You do not have enough money to buy the turret");
        }
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
        ShowButton(false);
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
        ShowButton(false);
        gameObject.SetActive(false);
    }

    public void ShowButton(bool show)
    {
        if (show)
        {
            purchaseButton.gameObject.SetActive(true);
            purchaseButton.GetComponentInChildren<Text>().text = "Purchase: " + BuildManager.Instance.GetTurretPrice().ToString();
        }
        else
        {
            purchaseButton.gameObject.SetActive(false);
        }
    }
    #endregion

}
