using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Turrets turretToBuild;

    #region Singleton
    public static BuildManager instance = null;
    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one buildManager");

        instance = this;
    }
    #endregion

    #region Monobehaviour
    private void Start()
    {
        
    }
    #endregion

    #region Functions
    public Turrets GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(Turrets.TurretType turretType)
    {
        
    }
    #endregion

}
