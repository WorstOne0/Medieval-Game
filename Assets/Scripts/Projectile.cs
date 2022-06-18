using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public LayerMask enemyCollisionMask, playerCollisionMask;
    public float speed = .5f, damage = 1;
    float lifeTime = 5, skinWidth = .1f;

    void Start() {
        Destroy(gameObject, lifeTime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, enemyCollisionMask);
        if (initialCollisions.Length > 0) {
            OnHitObject(initialCollisions[0]);
        }
    }

    void Update() {
        float moveDistance = speed * Time.deltaTime;
        CheckCollision(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    void CheckCollision(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, enemyCollisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, playerCollisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit) {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeDamage(damage, hit);
            Destroy(this.gameObject);
        }
    }

    void OnHitObject(Collider collider) {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.JustTakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
