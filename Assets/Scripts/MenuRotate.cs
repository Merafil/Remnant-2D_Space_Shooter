using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotate : MonoBehaviour {

    private float speedRotate = 45f;

    void FixedUpdate() {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }
}
