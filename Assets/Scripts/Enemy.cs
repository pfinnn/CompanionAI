using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{

    private void Start()
    {
        Health = 3000;
    }

    public int Health { get; set; }

    public void Damage(int dmg)
    {
        Health -= dmg;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
