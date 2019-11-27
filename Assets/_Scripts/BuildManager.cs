using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private Turrets turretToBuild;
    public Turrets[] turrets;

    #region Singleton
    public static BuildManager Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one buildManager");
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        turretToBuild = turrets[0];
    }

    public Turrets GetTurretToBuild()
    {
        return turretToBuild;
    }

    public float GetTurretPrice()
    {
        return turretToBuild.GetTurretCost();
    }

    public void SetTurretToBuild(Turrets.TurretType turretType)
    {
        foreach (Turrets turret in turrets)
        {
            if (turretType == turret.GetTurretType())
            {
                turretToBuild = turret;
                return;
            }
        }
        Debug.LogError("GONE WRONG");
    }
}
