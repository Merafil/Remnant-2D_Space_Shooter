using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public GameObject explosion;

    private AudioSource explosionSound;
    private CircleCollider2D _collider;
    private Renderer _renderer;
    private int maxAsteroidHealth = 4;
    private int currentAsteroidHealth;

    void Start() {
        explosionSound = GetComponent<AudioSource>();
        currentAsteroidHealth = maxAsteroidHealth;
        _collider = GetComponent<CircleCollider2D>();
        _renderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "Player") {
            if(currentAsteroidHealth > 0) {
                currentAsteroidHealth -= 1;
            } else {
                PlayExplosion();
                explosionSound.Play();
                _collider.enabled = !_collider.enabled;
                _renderer.enabled = !_renderer.enabled;
                GameController.SharedInstance.ScoreCalculator(5);
                Destroy(gameObject, 2f);
                currentAsteroidHealth = maxAsteroidHealth;
            }
        }
    }

        void OnTriggerExit2D(Collider2D other) {
            if(other.gameObject.tag == "Boundary") {
            Destroy(gameObject);
        }
    }

    void PlayExplosion() {
        GameObject explode = Instantiate(explosion);
        explode.transform.position = transform.position;
    }

}


