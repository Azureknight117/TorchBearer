using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for Player characters and enemies to inherit from

public class Actor : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    //currentHealth = maxHealth;
    public float movementSpeed;

    public GameObject ProjectileSpawn;
    public GameObject RightHandSocket;
    public GameObject BackSocket;
 
    public void SpawnProjectile(GameObject projectile)
    {
        Instantiate(projectile, ProjectileSpawn.transform.position, transform.rotation);
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void HealDamage(float healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("I dead");
    }
}
