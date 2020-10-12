using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public Turrets TurretToBuild { get; private set; }

    [SerializeField] private Turrets testTurret;

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
        TurretToBuild = testTurret;
    }
    #endregion

    #region Functions

    public void SetTurretToBuild(Turrets.TurretType turretType)
    {
        
    }
    #endregion

}
