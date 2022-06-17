using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 200;
    public float muzzleVelocity = 12;

    float nextShootTime;

    public void Shoot() {
        if (Time.time > nextShootTime) {
            nextShootTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);
        }
    }
}
