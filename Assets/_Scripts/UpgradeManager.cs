using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeManager : MonoBehaviour
{
    public delegate void PayForUpgrade(float cost);
    public event PayForUpgrade OnPay;

    private Button upgradeButton;
    private Turrets currentTurret = null;
    private bool upgradeCooldown;
    private Button sellButton;

    #region Singleton
    public static UpgradeManager Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one UpgradeManager");
        }
        else
        {
            Instance = this;
        }

        sellButton = FindObjectOfType<SellButon>().GetComponent<Button>();
        upgradeButton = FindObjectOfType<UpgradeButton>().GetComponent<Button>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CloseUpgrade();
    }

    public void OpenUpgrade(Turrets turret)
    {
        if (currentTurret == null)
        {
            currentTurret = turret;
            gameObject.SetActive(true);
            upgradeButton.GetComponentInChildren<Text>().text = "Upgrade: " + (currentTurret.GetTurretCost() * 1.2f).ToString();
            sellButton.GetComponentInChildren<Text>().text = currentTurret.GetTurretCost().ToString();
        }
        else
        {
            currentTurret = turret;
        }
    }

    public void CloseUpgrade()
    {
        currentTurret = null;
        gameObject.SetActive(false);
    }

    public void UpgradeTower()
    {
        if (currentTurret == null)
        {
            Debug.Log("No Turret selected. cannot upgrade");
            return;
        }

        if (GameManager.Instance.GetGold() >= currentTurret.GetTurretCost() * 1.2f)
        {
            currentTurret.RemoveBuffs();
            currentTurret.UpgradeTurret();
            if (OnPay != null)
            {
                OnPay(currentTurret.GetTurretCost() * 1.2f);
            }
        }
        else
        {
            Debug.Log("You dont have enough money to buy the upgrade!");
        }
        upgradeButton.GetComponentInChildren<Text>().text = "Upgrade: " + (currentTurret.GetTurretCost() * 1.2f).ToString();
    }

    public void SellTower()
    {
        if (currentTurret == null)
        {
            Debug.Log("No Turret selected. cannot sell");
            return;
        }

        if (OnPay != null)
        {
            OnPay(-currentTurret.GetTurretCost() * 1.2f);
        }
        currentTurret.GetComponentInParent<Node>().RemoveTower();

        Destroy(currentTurret.gameObject);
        CloseUpgrade();
    }
}
