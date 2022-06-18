using UnityEngine;

public interface IDamageable {
    void TakeDamage(float damage, RaycastHit hit);
    void JustTakeDamage(float damage);
}

