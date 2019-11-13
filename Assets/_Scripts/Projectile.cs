using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void HitEnemy(Turrets turret);
    public event HitEnemy SendBackToEnemy;
    public delegate void HitEnemy2(Projectile projectile);
    public event HitEnemy2 OnEnemyHit;
    public event HitEnemy2 OnSplashHit;

    private EnemySpawner enemySpawner;
    private float splashRange;
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
        enemySpawner = FindObjectOfType<EnemySpawner>();
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
            turret.OnSend -= Turret_OnSend;
            Destroy(gameObject);
        }
    }

    private void Turret_OnSend(Transform enemyTransform, float projectileSpeed, float splashRange)
    {
        target = enemyTransform;
        this.projectileSpeed = projectileSpeed;
        this.splashRange = splashRange;
    }

    private void OnTriggerStay(Collider other)
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


            if (splashRange > 0)
            {
                foreach (Enemy enemy in enemySpawner.enemyList)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < splashRange)
                    {
                        enemy.SplashSubscribeToTurret(this);
                        
                        if (SendBackToEnemy != null)
                        {
                            SendBackToEnemy(turret);
                        }
                        else
                        {
                            Debug.Log("Nothing subscribed to SplashSendBackToEnemy");
                        }

                        if (OnSplashHit != null)
                        {
                            OnSplashHit(this);
                        }
                        else
                        {
                            Debug.LogError("Nothing is subscribed to SplashOnDamageDealt");
                        }
                    }

                }
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
