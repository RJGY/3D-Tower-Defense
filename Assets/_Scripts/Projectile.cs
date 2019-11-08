using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void HitEnemy(Enemy enemy);
    public event HitEnemy OnEnemyHit;

    private float attackDamage;
    private float magicDamage;
    private float pureDamage;
    private float armourPenetration;
    private float magicResistPenetration;
    private float splashRange;
    private float slowAmount;
    private float projectileSpeed;

    public enum ProjectileArt
    {
        CannonBall,
        Arrow,
        FireBall,
        Lightning,
        Undefined
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (OnEnemyHit != null)
            {
                OnEnemyHit(enemy);
            }
            else
            {
                Debug.LogError("Nothing is subscribed to OnDamageDealt");
            }
        }
    }

    private void GoTowardsTargettedEnemy(Transform enemy)
    {

    }
}
