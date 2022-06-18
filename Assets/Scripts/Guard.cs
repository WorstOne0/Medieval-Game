using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : LivingEntity {
    public float speed = 5;
    public float waitTime = .3f, turnSpeed = 90, timeToSpotPlayer = 1f;

    Color originalSpotColor;
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    float viewAngle, playerVisibleTimer;

    public Transform pathHolder;
    Vector3 targetWaypoint;
    Transform player;
    LivingEntity playerEntity;
    GunController gunController;

    bool hasTarget = false;

    protected override void Start() {
        base.Start();

        gunController = GetComponent<GunController>();

        if (GameObject.FindGameObjectWithTag("Player") != null) {
            hasTarget = true;

            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerEntity = player.GetComponent<LivingEntity>();
            playerEntity.OnDeath += OnTargetDeath;
        }

        viewAngle = spotlight.spotAngle;
        originalSpotColor = spotlight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    void Update() {
        if (hasTarget) {
            if (CanSeePlayer()) {
                playerVisibleTimer += Time.deltaTime;
            } else {
                playerVisibleTimer -= Time.deltaTime;
                speed = 5;
            }

            playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
            // Blend from white to red
            spotlight.color = Color.Lerp(originalSpotColor, Color.red, playerVisibleTimer / timeToSpotPlayer);

            // If player visible for long enough
            if (playerVisibleTimer >= timeToSpotPlayer) {
                speed = 0;
                transform.LookAt(player.position);

                gunController.Shoot();
            }
        } else {
            speed = 5;
            spotlight.color = originalSpotColor;
        }
    }

    bool CanSeePlayer() {
        if (Vector3.Distance(transform.position, player.position) < viewDistance && player != null) {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
                if (!Physics.Linecast(transform.position, player.position, viewMask)) {
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator FollowPath(Vector3[] waypoints) {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

            if (transform.position == targetWaypoint) {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);

                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }

            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget) {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void OnDrawGizmos() {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder) {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    void OnTargetDeath() {
        hasTarget = false;
    }
}

