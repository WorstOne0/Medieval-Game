using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public LayerMask enemyCollisionMask, playerCollisionMask;
    public float speed = 0.5f;
    public float damage = 1;

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    void Update() {
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollision(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, enemyCollisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }

        // if (Physics.Raycast(ray, out hit, moveDistance, playerCollisionMask, QueryTriggerInteraction.Collide)) {
        //     OnHitObject(hit);
        // }
    }

    void OnHitObject(RaycastHit hit) {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeDamage(damage, hit);
            Destroy(this.gameObject);
        }
    }
}
