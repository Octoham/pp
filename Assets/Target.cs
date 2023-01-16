using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Target : NetworkBehaviour
{
    [Header("Health")]
    public float health;
    public float startHealth;
    public float maxHealth;
    [Header("Armor")]
    public float armor;
    public float startArmor;
    public float maxArmor;
    [Header("Other")]
    public bool invulnerable;

    void Start()
    {
        init();
    }
    void Update()
    {
        checkArmor();
        checkHealth();
    }

    public void init()
    {
        health = startHealth;
        armor = startArmor;
    }

    public void checkHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public void checkArmor()
    {
        if (armor > maxArmor)
        {
            armor = maxArmor;
        }
    }


    public void TakeDamage(float amount)
    {
        if (!invulnerable)
        {
            if ((armor * 2) < amount)
            {
                amount -= armor;
                armor -= armor;
                health -= amount;
            }
            else
            {
                armor -= amount / 2;
                health -= amount / 2;
            }
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        health = startHealth;
        armor = startArmor;
        gameObject.SetActive(true);
    }

}

