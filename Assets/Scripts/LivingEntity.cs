using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable {
    public float startingHealth = 3;
    protected float health;
    protected bool isDead;

    public event System.Action OnDeath;
    //public HealthBar healthBar;

    protected virtual void Start() {
        health = startingHealth;
    }

    public void TakeDamage(float damage, RaycastHit hit) {
        JustTakeDamage(damage);
    }

    public virtual void JustTakeDamage(float damage) {
        health -= damage;

        if (health <= 0 && !isDead) {
            Die();
        }
    }

    protected void Die() {
        isDead = true;

        if (OnDeath != null) {
            OnDeath();
        }

        GameObject.Destroy(gameObject);
    }
}
