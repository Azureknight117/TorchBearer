using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public GameObject target;

    void Start()
    {
        currentHealth = maxHealth;
    }

}
