using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {
    public float startingHealth = 3;
    protected float health;
    protected bool isDead;

    public virtual void Start() {
        health = startingHealth;
    }
    public void TakeDamage(float damage, RaycastHit hit) {
        health -= damage;

        if (health <= 0 && !isDead) {
            Die();
        }
    }

    public void Die() {
        isDead = true;
        GameObject.Destroy(gameObject);
    }
}
