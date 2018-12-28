using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject enemyBullet;
    public float intervalShootTime = 1.5f;
    public GameObject explosion;
    public List<GameObject> activeEnemyTurrets;
    public GameObject enemyDeathSound;

    private AudioSource enemyShootSound;
    private Vector2 pos;

    void Start () {
        enemyShootSound = GetComponent<AudioSource>();
        pos = new Vector2(0, 0);

	}

    private void OnEnable()
    {
        StartCoroutine("Shoot");
    }

    private void Update() {
        pos = transform.position;
        transform.position = pos;
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(intervalShootTime);
        while(true) {
            foreach(GameObject cannon in activeEnemyTurrets) {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
                if(bullet != null) {
                    bullet.transform.position = cannon.transform.position;
                    bullet.transform.rotation = cannon.transform.rotation;
                    bullet.SetActive(true);
                    enemyShootSound.Play();
                    yield return new WaitForSeconds(intervalShootTime);
                }
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Boundary") {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "PlayerBullet" || other.gameObject.tag == "Player") {
            GameController.SharedInstance.ScoreCalculator(10);
            gameObject.SetActive(false);
            PlayExplosion();
            Instantiate(enemyDeathSound);
        }
    }

    void PlayExplosion() {
        GameObject explode = Instantiate(explosion);
        explode.transform.position = transform.position;
    }

}


