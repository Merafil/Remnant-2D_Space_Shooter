using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

    public float speed;

    private Rigidbody2D body;
   
    void OnEnable() {
        body = transform.GetComponent<Rigidbody2D>();
        body.velocity = transform.up * speed;
    }
}
