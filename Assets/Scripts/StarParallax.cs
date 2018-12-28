using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParallax : MonoBehaviour {

    public float speed;

    private Rigidbody2D starsBody;

    void Start() {
        starsBody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        starsBody.velocity = new Vector2(0,-speed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Parallax") {
            this.transform.position = new Vector2(0.735f, 85);
        }  
    }
}
