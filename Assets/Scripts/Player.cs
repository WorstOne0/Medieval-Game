using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 7;
    public float smoothMoveTime = .1f, turnSpeed = 8;

    float angle, smoothInputMagnitude, smoothMoveVelocity;

    Vector3 velocity;
    Rigidbody myRigidbody;

    Camera viewCamera;
    GunController gunController;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    void Update() {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            Vector3 adjustedPoint = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(adjustedPoint);
        }

        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.unscaledDeltaTime * inputMagnitude);

        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
        //velocity = inputDirection.normalized * moveSpeed * smoothInputMagnitude;

        if (Input.GetMouseButton(0)) {
            gunController.Shoot();
        }
    }

    void FixedUpdate() {
        myRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedUnscaledDeltaTime);
    }
}
