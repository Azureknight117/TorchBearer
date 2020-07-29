using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base Class for enemies to inherit from
public class Enemy : Actor
{
    public GameObject target;

    void Start()
    {
        currentHealth = maxHealth;
    }

}
