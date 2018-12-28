using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarathController : MonoBehaviour {

    public List<GameObject> activeBossCannons;
    public List<GameObject> activeFireBlasters;
    public float intervalShootTime = 0.1f;
    public float intervalFireBlastShootTime = 5f ;
    public GameObject explosion;
    public GameObject audioBoss;
    public GameObject audioBossLine;
    public GameObject audioLaser;

    public float maxBossHealth = 200;
    public float currentBossHealth = 0;
  //  public bool isAlive;

    private Vector2 pos;

    void Start() {
        StartCoroutine("Shoot");
        StartCoroutine("FireBlast");
        currentBossHealth = maxBossHealth;
        pos = new Vector2(0, 0);
        Instantiate(audioBossLine);
}
    private void OnEnable() {
    //    currentBossHealth = maxBossHealth;
    }

    void Update() {
        pos = transform.position;
        transform.position = pos;
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(intervalShootTime );
        for( ; ; ) {
            foreach(GameObject cannon in activeBossCannons) {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
                if(bullet != null) {
                    bullet.transform.position = cannon.transform.position;
                    bullet.transform.rotation = cannon.transform.rotation;
                    bullet.SetActive(true);
                    yield return new WaitForSeconds(intervalShootTime);
                }
            }
        }
    }

  IEnumerator FireBlast() {
        for(; ; ) {
            yield return new WaitForSeconds(intervalFireBlastShootTime);
            foreach(GameObject cannon in activeFireBlasters) {
                GameObject rocket = ObjectPooler.SharedInstance.GetPooledObject("EnemyLaser");
                if(rocket != null) {
                    rocket.transform.position = cannon.transform.position;
                    rocket.transform.rotation = cannon.transform.rotation;
                    rocket.SetActive(true);
                    Instantiate(audioLaser);
                }
            }
        }
    } 

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "PlayerBullet" ) {
            if(currentBossHealth <= 0) {
                GameController.SharedInstance.isBossAlive = false;
                Debug.Log(GameController.SharedInstance.isBossAlive);
                Instantiate(audioBoss);
                PlayExplosion();
                Destroy(gameObject);

            } else {
                currentBossHealth -= 1;
            }
        }
    }

    void PlayExplosion() {
        GameObject explode = Instantiate(explosion);
        explode.transform.position = transform.position;
    }
 }

