using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

public class PlayerController : MonoBehaviour {

    [System.Serializable]
    public class Boundary {
        public float xMin, xMax, yMin, yMax;
    }

    public GameObject firstHeart;
    public GameObject secondHeart;
    public GameObject thirdHeart;
    public GameObject shield;
    public float speed;
    public Boundary boundary;
    public GameObject playerBullet;
    public GameObject startWeapon;
    public GameObject explosion;
    public List<GameObject> activePlayerCannons;
    public List<GameObject> firstUpgrade;           
    public List<GameObject> finalUpgrade;
    public float fireRate;
    public float invicibilityTime;
    public GameObject deathSound;
    public GameObject hitSound;


    public int upgradeStatus = 0;

    private int maxHealth = 3;
    private float timer = 0f;
    private int currentHealth;
   // private Renderer playerRenderer;                          
  //  private CapsuleCollider2D playerCollider;
    private Rigidbody2D playerRigidBody;
    private AudioSource shootSound;
 //   private AudioSource deadSound;
    private float fireInterval;
    private Vector2 pos;
    private bool isShieldActive = false ;


    void Start() {
    //    playerCollider = gameObject.GetComponent<CapsuleCollider2D>();
    //    playerRenderer = gameObject.GetComponent<Renderer>();
        playerRigidBody = gameObject.GetComponent<Rigidbody2D>();
        activePlayerCannons = new List<GameObject>();
        shootSound = gameObject.GetComponent<AudioSource>();
   //     deadSound = gameObject.GetComponent<AudioSource>();
        activePlayerCannons.Add(startWeapon);
        currentHealth = maxHealth;
        pos = new Vector2(0,0);
    }

    void Update() {

        timer += Time.deltaTime;
        checkHealth(currentHealth);
        if(Input.GetKey("space") && Time.time > fireInterval) {
            Shoot();
            }
        }

    void Shoot() {
        timer += Time.deltaTime;
        foreach(GameObject cannon in activePlayerCannons) { 
            fireInterval = Time.time + fireRate;
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("PlayerBullet");
            if(bullet != null) {
                bullet.transform.position = cannon.transform.position;
                bullet.transform.rotation = cannon.transform.rotation;
                bullet.SetActive(true);
            }
        }  shootSound.Play();
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        playerRigidBody.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);
        playerRigidBody.position = new Vector2(Mathf.Clamp(playerRigidBody.position.x, boundary.xMin, boundary.xMax),
                                               Mathf.Clamp(playerRigidBody.position.y, boundary.yMin, boundary.yMax));
        pos = transform.position;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "PowerUp") {
            CollectPowerUp powerUpScript = other.gameObject.GetComponent<CollectPowerUp>();
            powerUpScript.PowerUpCollected();
            UpgradeWeapons();
        }

        if(isShieldActive && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Asteroid" || other.gameObject.tag == "EnemyBullet" || 
                              other.gameObject.tag == "EnemyLaser")) {
            Instantiate(hitSound);
            isShieldActive = false;
            shield.SetActive(false);
        } else if(timer >= invicibilityTime && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Asteroid" || 
                  other.gameObject.tag == "EnemyBullet" || other.gameObject.tag == "EnemyLaser")) {
            Damage();
            if(currentHealth.Equals(2)) {
                Instantiate(hitSound);
                thirdHeart.gameObject.SetActive(false);
            } else if(currentHealth.Equals(1)) {
                Instantiate(hitSound);
                secondHeart.gameObject.SetActive(false);
            } else {
                firstHeart.gameObject.SetActive(false);
                PlayExplosion();
                Instantiate(deathSound);
                gameObject.SetActive(false);
                GameController.SharedInstance.ShowGameOverNotification();
            }
            timer = 0;
        }

    }

    void PlayExplosion() {
        GameObject explode = Instantiate(explosion);
        explode.transform.position = transform.position;
    }

    private void OnTriggerStay2D(Collider2D other) {

        if(other.gameObject.tag == "HealthRestore" && currentHealth < 3) {
            CollectPowerUp powerUpScript = other.gameObject.GetComponent<CollectPowerUp>();
            powerUpScript.PowerUpCollected();
            currentHealth += 1;
        }

        if(other.gameObject.tag == "ShieldPowerUp" && !isShieldActive) {
            shield.SetActive(true);
            CollectPowerUp shieldUpScript = other.gameObject.GetComponent<CollectPowerUp>();
            isShieldActive = true;
         //   Debug.Log(isShieldActive);
            shieldUpScript.PowerUpCollected();
            } 
  
        if(timer >= invicibilityTime && other.gameObject.tag == "Boss") {
            Damage();
            if(currentHealth.Equals(2)) { thirdHeart.gameObject.SetActive(false); } 
            else if(currentHealth.Equals(1)) { secondHeart.gameObject.SetActive(false); } 
            else {
                firstHeart.gameObject.SetActive(false);
                PlayExplosion();
                Instantiate(deathSound);
                gameObject.SetActive(false);
                GameController.SharedInstance.ShowGameOverNotification();
            }
            timer = 0;
        }
    }

    void checkHealth(int health) {
        if(currentHealth == 3) {
            thirdHeart.gameObject.SetActive(true);
        }
        else if(currentHealth == 2) {
            secondHeart.gameObject.SetActive(true);
        }
        else if(currentHealth == 1) {
            firstHeart.gameObject.SetActive(true);
        }
    }

    void Damage() {   
            if(currentHealth > 0) 
                currentHealth -= 1;
        }

    private void UpgradeWeapons() {
        if(upgradeStatus == 0) {
            foreach(GameObject cannon in firstUpgrade) {
                activePlayerCannons.Add(cannon);
            }
        } else if(upgradeStatus == 1) {
            foreach(GameObject cannon in finalUpgrade) {
                activePlayerCannons.Add(cannon);
            }
        } else {
            return;
        }
        upgradeStatus++;
    }
}