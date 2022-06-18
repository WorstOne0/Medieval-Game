using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : LivingEntity {
    public enum State {
        Idle,
        Chasing,
        Attacking,
    }
    State currentState;

    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    float attackDistanceThreshold = 1.5f, timeBetweenAttacks = 2f, nextAttackTime;
    float myCollisionRadius, targetCollisionRadius;

    bool hasTarget = false;

    protected override void Start() {
        base.Start();

        if (GameObject.FindGameObjectWithTag("Player") != null) {
            currentState = State.Chasing;
            hasTarget = true;

            pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }
    }

    void Update() {
        if (hasTarget) {
            if (Time.time > nextAttackTime) {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack() {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = new Vector3(target.position.x - dirToTarget.x * (myCollisionRadius), transform.position.y, target.position.z - dirToTarget.z * (myCollisionRadius));

        float percent = 0, attackSpeed = 3;
        bool hasAppliedDamage = false;

        while (percent <= 1) {
            if (percent >= .5f && !hasAppliedDamage) {
                hasAppliedDamage = true;
                targetEntity.JustTakeDamage(1);
            }


            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath() {
        float refreshRate = 0.25f;

        while (hasTarget) {
            if (currentState == State.Chasing) {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);

                if (!isDead) {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    void OnTargetDeath() {
        hasTarget = false;
        currentState = State.Idle;
    }
}
