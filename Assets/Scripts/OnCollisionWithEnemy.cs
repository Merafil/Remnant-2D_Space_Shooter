using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionWithEnemy : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Asteroid" || other.gameObject.tag == "Boss") {
            gameObject.SetActive(false);
        }
    }
}
