using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPowerUp : MonoBehaviour {

    private CircleCollider2D powerUpCollider;
    private Renderer powerUpRenderer;


    void Start() {
        powerUpCollider = gameObject.GetComponent<CircleCollider2D>();
        powerUpRenderer = gameObject.GetComponent<Renderer>();
    }

    public void PowerUpCollected() {
        powerUpCollider.enabled = false;
        powerUpRenderer.enabled = false;
    }
}
 
