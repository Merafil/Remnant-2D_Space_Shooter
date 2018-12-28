using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController SharedInstance;

    [SerializeField]
    GameObject[] powerUpSpawns;

    public Text scoreNotification;
    public Text highScoreNotification;
    public Text gameOverNotification;
    public Button backtoMenuButton;
    public Button tryAgainButton;

    private Animator animBoss;

    public float waveInterval = 4f;
    public float spawnShipsInterval = 0.6f;
    public float spawnAsteroidsInterval = 0.25f;
    public GameObject enemyShip;
    public GameObject meteorytes;

    private int currentScore = 0;
    private int highScore = 0;

    public bool isBossAlive = true;

    public GameObject weaponPowerUp;
    public GameObject healthPowerUp;
    public GameObject shieldPowerUp;

    private float powerUpSpawnTime = 10;
    private float currentPowerUpSpawnTime = 0;

    public GameObject asteroid;
    private GameObject newPowerUp;


    public GameObject boss;
    private bool bossSpawned = false;
    private bool isCoroutineStarted = false;
    private int bossesKilled = 0;

    void Awake() {
        SharedInstance = this;
    }

    void Start() {
        StartCoroutine("SpawnWaves");
        StartCoroutine("PowerUpSpawn");
        LoadHighScore();
    }

    void Update() {
        Debug.Log(isBossAlive);

        currentPowerUpSpawnTime += Time.deltaTime;

        if(IsNewHighScore(currentScore)) {
            SetHighScore(currentScore);
        }

      if (currentScore >= 100 * (bossesKilled + 1) && isBossAlive && !bossSpawned) {
            bossSpawned = true;
            StopCoroutine("SpawnWaves");
            StopCoroutine("SpawnWavesAgain");
            Instantiate(boss);
            isCoroutineStarted = false;
        }

        Debug.Log(isCoroutineStarted);

        if(!isBossAlive &&  bossSpawned  && !isCoroutineStarted) {
            isBossAlive = true;
            bossSpawned = false;
            bossesKilled += 1;
            StartCoroutine("SpawnWavesAgain");
            
        }
    }

    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(0.5f);
        for( ; ; ) {
            float waveType = Random.Range(0f, 2f);
            int enemiesPerWave = Random.Range(10, 14);
            int asteroidsPerWave = Random.Range(30, 45);
            if(waveType >= 1) {
                for(int i = 0; i < enemiesPerWave; i++) {
                    Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(30, Camera.main.pixelHeight + 2, 0));
                    Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 30, Camera.main.pixelHeight + 2, 0));
                    Vector3 spawnPosition = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y, 0);
                    Quaternion spawnRotation = Quaternion.Euler(0, 0, 180);
                    GameObject enemyShip = ObjectPooler.SharedInstance.GetPooledObject("Enemy");
                    if(enemyShip != null) {
                        enemyShip.transform.position = spawnPosition;
                        enemyShip.transform.rotation = spawnRotation;
                        enemyShip.SetActive(true);
                    }
                    yield return new WaitForSeconds(spawnShipsInterval);
                }
            } else {
                for(int i = 0; i < asteroidsPerWave; i++) {
                    Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(50, Camera.main.pixelHeight + 2, 0));
                    Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 50, Camera.main.pixelHeight + 2, 0));
                    Vector3 spawnPosition = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y, 0);
                    Quaternion spawnRotation = Quaternion.Euler(0, 0, 180);
                    Instantiate(asteroid,spawnPosition,spawnRotation);
                    yield return new WaitForSeconds(spawnAsteroidsInterval);
                }
            }
            yield return new WaitForSeconds(waveInterval);
        }
    }

    IEnumerator SpawnWavesAgain() {
        isCoroutineStarted = true;
        yield return new WaitForSeconds(1.5f);
        for(; ; ) {
            float waveType = Random.Range(0f, 2f);
            int enemiesPerWave = Random.Range(10, 14);
            int asteroidsPerWave = Random.Range(30, 45);
            if(waveType >= 1) {
                for(int i = 0; i < enemiesPerWave; i++) {
                    Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(30, Camera.main.pixelHeight + 2, 0));
                    Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 30, Camera.main.pixelHeight + 2, 0));
                    Vector3 spawnPosition = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y, 0);
                    Quaternion spawnRotation = Quaternion.Euler(0, 0, 180);
                    GameObject enemyShip = ObjectPooler.SharedInstance.GetPooledObject("Enemy");
                    if(enemyShip != null) {
                        enemyShip.transform.position = spawnPosition;
                        enemyShip.transform.rotation = spawnRotation;
                        enemyShip.SetActive(true);
                    }
                    yield return new WaitForSeconds(spawnShipsInterval);
                }
            } else {
                for(int i = 0; i < asteroidsPerWave; i++) {
                    Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(50, Camera.main.pixelHeight + 2, 0));
                    Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 50, Camera.main.pixelHeight + 2, 0));
                    Vector3 spawnPosition = new Vector3(Random.Range(topLeft.x, topRight.x), topLeft.y, 0);
                    Quaternion spawnRotation = Quaternion.Euler(0, 0, 180);
                    Instantiate(asteroid, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnAsteroidsInterval);
                }
            }
            yield return new WaitForSeconds(waveInterval);
        }
    }
    IEnumerator PowerUpSpawn() {
        if(currentPowerUpSpawnTime > powerUpSpawnTime) {
            currentPowerUpSpawnTime = 0;

            int randomNumber = Random.Range(0, powerUpSpawns.Length );

            GameObject spawnLocation = powerUpSpawns[randomNumber];
            int randomPowerUp = Random.Range(0, 100);
            if(randomPowerUp <=  10 ) {
                newPowerUp = Instantiate(weaponPowerUp) as GameObject;
            } else if(randomPowerUp <= 40) {
                newPowerUp = Instantiate(healthPowerUp) as GameObject;
            } else if(randomPowerUp <= 100) {
                newPowerUp = Instantiate(shieldPowerUp) as GameObject;
            }  newPowerUp.transform.position = spawnLocation.transform.position;
            
        }
    yield return null ;
        StartCoroutine(PowerUpSpawn());
} 

        public void ScoreCalculator(int points) {
        currentScore += points;
        scoreNotification.text = "Score:" + currentScore;
    }

    public void LoadHighScore() {
        highScore = PlayerPrefs.GetInt("HIGHSCORE");
        highScoreNotification.text = "HighestScore:" + highScore.ToString();
    }

    public bool IsNewHighScore(int scoreValue) {
        int storedValue = PlayerPrefs.GetInt("HIGHSCORE");
        return (scoreValue > storedValue);
    }

    public void SetHighScore(int newScore) {
        int storedValue = PlayerPrefs.GetInt("HIGHSCORE");
        if(newScore > storedValue) {
            PlayerPrefs.SetInt("HIGHSCORE", newScore);
            highScoreNotification.text = "HighestScore:" + newScore.ToString();
        }
    }

    public void ShowGameOverNotification() {
        StopAllCoroutines();
        gameOverNotification.rectTransform.anchoredPosition3D = new Vector3(15, 30, 0);
        backtoMenuButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(90, -20, 0);
        tryAgainButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-90, -20, 0);
    }

    public void TryAgain() {
        SceneManager.LoadScene("Gameplay");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("GameMenu");
    }
}

