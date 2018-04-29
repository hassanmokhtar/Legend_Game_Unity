using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	[SerializeField] GameObject player;
	[SerializeField] GameObject[] spawnPoints;
	[SerializeField] GameObject[] powerUpSpawns;
	[SerializeField] GameObject Monster;
	[SerializeField] GameObject Skeleton;
	[SerializeField] GameObject Troll;
	[SerializeField] GameObject healthPowerUp;
	[SerializeField] GameObject speedPowerUp;
	[SerializeField] Text levelText;
	[SerializeField] Text endGameText;
	[SerializeField] int maxPowerUps = 4;
	[SerializeField] int finalLevel = 20;

	private bool gameOver = false;
	private int currentLevel;
	private float generatedSpawnTime = 1;    // wait 1 sec between every spawn
	private float currentSpawnTime = 0;
	private float powerUpSpawnTime = 60;
	private float currentPowerUpSpawnTime = 0;
	private GameObject newEnemy;
	private int powerups = 0;
	private GameObject newPowerup;

	private List<EnemyHealth> enemies = new List<EnemyHealth> ();   // keep track of spwaned enemies
	private List<EnemyHealth> killedEnemies = new List<EnemyHealth> ();   // keep track of killed enemies

	public void RegisterEnemy(EnemyHealth enemy) {
		enemies.Add (enemy);
	}

	public void KilledEnemy(EnemyHealth enemy) {
		killedEnemies.Add (enemy);
	}

	public void RegisterPowerUp() {
		powerups++;
	}

	public bool GameOver {
		get {return gameOver; }
	}

	public GameObject Player {
		get { return player; }
	}

	void Awake() {

		if (instance == null) {
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {

		endGameText.GetComponent<Text> ().enabled = false;
		StartCoroutine (spawn ());
		StartCoroutine (powerUpSpawn ());
		currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {

		currentSpawnTime += Time.deltaTime;      // keep track of how long since we passed spawn time
		currentPowerUpSpawnTime += Time.deltaTime;
	}

	public void PlayerHit(int currentHP) {

		if (currentHP > 0) {
			gameOver = false;
		} else {
			gameOver = true;
			StartCoroutine (endGame ("Defeat"));
		}
	}

	IEnumerator spawn() {

		if (currentSpawnTime > generatedSpawnTime) {  // law el spawn el nzlt b2alha ftra aktr mn 1 sec ms el w2t bta3 nzol el enemy el 2a5er
			currentSpawnTime = 0;    // counter restart

			if (enemies.Count < currentLevel) {   // if there are enemies on screen < current level  --> select spawn point and spawn rondom enemy

				int randomNumber = Random.Range (0, spawnPoints.Length - 1);
				GameObject spawnLocation = spawnPoints [randomNumber];    // rondomly pick 1 of the 4 spawn points to clone from it

				int randomEnemy = Random.Range (0, 3);
				if (randomEnemy == 0) {
					newEnemy = Instantiate (Troll) as GameObject;     // so we create objects from prefabs
				} else if (randomEnemy == 1) {
					newEnemy = Instantiate (Monster) as GameObject;
				} else if (randomEnemy == 2) {
					newEnemy = Instantiate (Skeleton) as GameObject;
				}

				newEnemy.transform.position = spawnLocation.transform.position;  // set position of the new spawned enemy to the same location of the spawn point
					
			}

			if (killedEnemies.Count == currentLevel && currentLevel != finalLevel) {    // if we killed the same number  of enemies as the current level, clear the enemies and killed enemies arrays, incremenr level by 1

				enemies.Clear ();
				killedEnemies.Clear ();

				yield return new WaitForSeconds (3f);  // wait for 3 seconds before jumg to next level
				currentLevel++;
				levelText.text = "Level " + currentLevel;
			}

			if (killedEnemies.Count == finalLevel) {
				StartCoroutine (endGame ("Victory!"));
			}
		}

		yield return null; 
		StartCoroutine (spawn ());  // recursive
	}

	IEnumerator powerUpSpawn() {

		if (currentPowerUpSpawnTime > powerUpSpawnTime) {
			currentPowerUpSpawnTime = 0;

			if (powerups < maxPowerUps) {

				int randomNumber = Random.Range (0, powerUpSpawns.Length - 1);
				GameObject spawnLocation = powerUpSpawns [randomNumber];  

				int randomPowerUp = Random.Range (0, 2);
				if (randomPowerUp == 0) {
					newPowerup = Instantiate (healthPowerUp) as GameObject;
				} else if (randomPowerUp == 1) {
					newPowerup = Instantiate (speedPowerUp) as GameObject;
				}

				newPowerup.transform.position = spawnLocation.transform.position;
			}
		}

		yield return null;
		StartCoroutine (powerUpSpawn ());
	}

	IEnumerator endGame(string outcome) {

		endGameText.text = outcome;
		endGameText.GetComponent<Text> ().enabled = true;
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene ("GameMenu");
	}
}
