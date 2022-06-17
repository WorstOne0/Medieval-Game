using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity {
    public float moveSpeed = 7;
    public float smoothMoveTime = .1f, turnSpeed = 8;

    float angle, smoothInputMagnitude, smoothMoveVelocity;

    Vector3 velocity;
    Rigidbody myRigidbody;

    Camera viewCamera;
    GunController gunController;
    FlagController flagController;

    public override void Start() {
        base.Start();
        myRigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        flagController = GetComponent<FlagController>();
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

        Matrix4x4 targetRotation = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        Vector3 skewedInput = targetRotation.MultiplyPoint3x4(inputDirection);

        //float targetAngle = Mathf.Atan2(skewedInput.x, skewedInput.z) * Mathf.Rad2Deg;
        //angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.unscaledDeltaTime * inputMagnitude);

        velocity = skewedInput * moveSpeed * smoothInputMagnitude;
        //velocity = transform.forward * moveSpeed * smoothInputMagnitude;

        if (Input.GetMouseButton(0)) {
            gunController.Shoot();
        }
    }

    void OnTriggerEnter(Collider hitCollider) {
        if (hitCollider.tag == "Finish") {
            FunctionTimer.Create(flagController.EquipFlag, 3f);
        }

        if (hitCollider.tag == "Respawn") {
            FunctionTimer.Create(flagController.UnequipFlag, 1f);
        }
    }

    void FixedUpdate() {
        //myRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
