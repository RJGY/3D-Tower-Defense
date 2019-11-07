using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public delegate void DoDamage(float attackDamage, float armourPenetration, float magicDamage, float magicResistPenetration, float pureDamage);
    public event DoDamage OnDamageDealt;

    private float attackDamage;
    private float magicDamage;
    private float pureDamage;
    private float armourPenetration;
    private float magicResistPenetration;
    private float splashRange;
    private float slowAmount;
    private float projectileSpeed;

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
            if (OnDamageDealt != null)
            {
                OnDamageDealt(attackDamage, armourPenetration, magicDamage, magicResistPenetration, pureDamage);
            }
            else
            {
                Debug.LogError("Nothing is subscribed to OnDamageDealt");
            }
        }
    }
}
