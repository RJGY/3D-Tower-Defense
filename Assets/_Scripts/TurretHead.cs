using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHead : MonoBehaviour
{
    private Turrets turret;

    private void Awake()
    {
        turret = GetComponentInParent<Turrets>();
    }
    
    void Start()
    {
        turret.OnLookAtEnemy += Turret_OnLookAtEnemy;
    }

    private void Turret_OnLookAtEnemy(Vector3 enemyDirection)
    {
        transform.rotation = Quaternion.LookRotation(enemyDirection);
        transform.eulerAngles -= new Vector3(-90, 0, 0);
    }
}
