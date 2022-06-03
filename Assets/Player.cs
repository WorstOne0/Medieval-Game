using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 7;

    void Update() {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * targetAngle;

        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * inputMagnitude, Space.World);
    }
}
