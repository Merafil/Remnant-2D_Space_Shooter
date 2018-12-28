using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float moveSpeed = 5f;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    private float tChange = 0.0f;
    private float randomX;
    private float randomY;

    void Start() {
    }

    void Update() {
        if(Time.time >= tChange) {
            randomX = Random.Range(-2.0f, 2.0f);
            randomY = Random.Range(-0.5f, 1f);

            tChange = Time.time + Random.Range(1f, 2f);
        }
        Vector3 newPosition = new Vector3(randomX, randomY, 0);
        transform.Translate(newPosition * moveSpeed * Time.deltaTime);

        if(transform.position.x >= maxX || transform.position.x <= minX) {
            randomX = -randomX;
        }
        if(transform.position.y >= maxY || transform.position.y <= minY) {
            randomY = -randomY;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss") {
            randomX = Random.Range(-1.0f, 1.0f);
            randomY = Random.Range(-0.5f, 1f);
        }
    }
}