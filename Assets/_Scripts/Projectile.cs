using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void HitEnemy(Turrets turret);
    public event HitEnemy SendBackToEnemy;
    public delegate void HitEnemy2(Projectile projectile);
    public event HitEnemy2 OnEnemyHit;

    private int splashRange;
    private float projectileSpeed;
    private Turrets turret;
    private Transform target;

    public enum ProjectileArt
    {
        CannonBall,
        Arrow,
        FireBall,
        Lightning,
        Undefined
    }

    // Start is called before the first frame update
    void Awake()
    {
        turret = GetComponentInParent<Turrets>();
        turret.OnSend += Turret_OnSend;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            GoTowardsTargettedEnemy(target);
        }
        else
        {
            Debug.Log("No Target");
        }
    }

    private void Turret_OnSend(Transform enemyTransform, float projectileSpeed, int splashRange)
    {
        target = enemyTransform;
        this.projectileSpeed = projectileSpeed;
        this.splashRange = splashRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (SendBackToEnemy != null)
            {
                SendBackToEnemy(turret);
            }
            else
            {
                Debug.Log("Nothing subscribed to SendBackToEnemy");
            }
            
            if (OnEnemyHit != null)
            {
                OnEnemyHit(this);
            }
            else
            {
                Debug.LogError("Nothing is subscribed to OnDamageDealt");
            }
            turret.OnSend -= Turret_OnSend;

            Destroy(gameObject);
        }
    }

    private void GoTowardsTargettedEnemy(Transform enemy)
    {
        transform.position = Vector3.MoveTowards(transform.position, enemy.position, projectileSpeed * Time.deltaTime);
    }
}
