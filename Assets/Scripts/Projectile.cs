using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10;

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
